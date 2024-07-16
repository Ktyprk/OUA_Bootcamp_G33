using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    private Animator _animator;
    private ThirdPersonActionAsset playerActionAsset;
    private InputAction move, aim, fire, sprint , look, interact, inventory, closeUi;
    public bool isjump;
    public bool issprint;
    
    
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    
    public Image staminaBar;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    private ISearchable iSearchable;
    private IItemCollectible iCollectible;
    private IItemAvailability iItemAvailability;
    private CharacterController _controller;
    
    private float _speed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5.335f;
    public float SpeedChangeRate = 10.0f;
    
    private bool isAim = false;
    private bool isUIopen = false;
    
    [SerializeField] private float Gravity = -15.0f;
    
    [SerializeField] private float stamina = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float sprintStaminaCost = 10f;
    [SerializeField] private float jumpStaminaCost = 20f;
    [SerializeField] private float staminaRecoveryRate = 8f;
    
    private bool isJumping = false;
    
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    
    public float JumpHeight = 1.2f;
    public float JumpTimeout = 0.50f;
    public float FallTimeout = 0.15f;
    
    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;
    
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    
    // animation IDs
    private float _animationBlend;
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    
    private bool _hasAnimator;

    private void Awake()
    {
        playerActionAsset = new ThirdPersonActionAsset();
        move = playerActionAsset.Player.Move;
        look = playerActionAsset.Player.Look;
        interact = playerActionAsset.Player.Interact;
        inventory = playerActionAsset.Player.Inventory;
        aim = playerActionAsset.Player.Aim;
        fire = playerActionAsset.Player.Fire;
        closeUi = playerActionAsset.Player.CloseButton;
    }

    private void Start()
    {
        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();
        
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
        
        AssignAnimationIDs();
    }

    private void OnEnable()
    {
        playerActionAsset.Enable();
        look.performed += Look;
        look.canceled += Look;
        interact.performed += Interact;
        inventory.performed += Inventory;
        aim.performed += Aim;
        aim.canceled += StopAiming;
        fire.performed += Fire;
        closeUi.performed += CloseInventory;
    }

    private void OnDisable()
    {
        playerActionAsset.Disable();
        look.performed -= Look;
        look.canceled -= Look;
        interact.performed -= Interact;
        inventory.performed -= Inventory;
        aim.performed -= Aim;
        aim.canceled -= StopAiming;
        fire.performed -= Fire;
        
    }
    
    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }
    
    public void JumpInput(bool newJumpState)
    {
        isjump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        issprint = newSprintState;
    }
    
    private void Fire(InputAction.CallbackContext obj)
    {
        ItemScript equippedWeapon = InventoryManager.instance.GetEquippedWeapon();
        if (equippedWeapon != null)
        {
            equippedWeapon.Fire();
            
        }
        else
        {
            
        }
    }

    private void Update()
    {
        //HandleStamina();/
        JumpAndGravity();
        GroundedCheck();
        Move();
        if (!isAim && !isUIopen)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            LookAt(move.ReadValue<Vector2>());
        }
        else if (isAim && !isUIopen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            LookAtMouse();
        }else if (isUIopen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }
    
    private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;
                
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
                
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }
                
                if (isjump && _jumpTimeoutDelta <= 0.0f)
                {
                     _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }
                
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;
                
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }
                
                isjump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
    
    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
        
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);
        }
    }

     private void Move()
    { 
        float targetSpeed = issprint ? SprintSpeed : MoveSpeed;

        if (move.ReadValue<Vector2>() == Vector2.zero) targetSpeed = 0.0f;
        
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = move.ReadValue<Vector2>().magnitude;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
         
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }
        
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        Vector3 inputDirection = new Vector3(move.ReadValue<Vector2>().x, 0.0f, move.ReadValue<Vector2>().y).normalized;

       if (move.ReadValue<Vector2>() != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              playerCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                rotationSpeed);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }



    private void LookAt(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            Vector3 forward = freeLookCamera.transform.forward;
            Vector3 right = freeLookCamera.transform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 direction = input.x * right + input.y * forward;
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    private void LookAtMouse()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = playerCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = (hit.point - transform.position).normalized;
            direction.y = 0; 

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    

    private void Look(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 300f;
        }
        else if (context.canceled)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 0f;
        }
    }
    
    private void Aim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAim = true;
        }
    }

    private void StopAiming(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            isAim = false;
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        iCollectible?.Collect();
        iCollectible = null;
        iSearchable?.Search();
        iItemAvailability?.UseItem();
    }
    
    private void Inventory(InputAction.CallbackContext context)
    {
        isUIopen = !isUIopen;
        InventoryManager.instance.OpenInventory();
    }
    
    private void CloseInventory(InputAction.CallbackContext context)
    {
       // isUIopen = false;
       // InventoryManager.instance.CloseInventory();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ISearchable>() != null)
        {
            iSearchable = other.GetComponent<ISearchable>();
        }
        
        if (other.GetComponent<IItemCollectible>() != null)
        {
            iCollectible = other.GetComponent<IItemCollectible>();
        }
          
        if (other.GetComponent<IItemAvailability>() != null)
        {
            iItemAvailability = other.GetComponent<IItemAvailability>();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ISearchable>() != null && other.GetComponent<ISearchable>() == iSearchable)
        {
            iSearchable = null;
        }
        
        if (other.GetComponent<IItemCollectible>() != null && other.GetComponent<IItemCollectible>() == iCollectible)
        {
            iCollectible = null;
        }
        
        if (other.GetComponent<IItemAvailability>() != null && other.GetComponent<IItemAvailability>() == iItemAvailability)
        {
            iItemAvailability = null;
        }
    } 
    
    // private void HandleStamina()
    // {
    //     if (isSprinting && stamina > 0)
    //     {
    //         stamina -= sprintStaminaCost * Time.deltaTime;
    //         if (stamina <= 0)
    //         {
    //             stamina = 0;
    //             isSprinting = false;
    //         }
    //     }
    //     else if (!isSprinting && stamina < maxStamina)
    //     {
    //         stamina += staminaRecoveryRate * Time.deltaTime;
    //         if (stamina > maxStamina)
    //         {
    //             stamina = maxStamina;
    //         }
    //     }
    //     staminaBar.fillAmount = stamina / maxStamina;
    //     
    //     if(stamina >= maxStamina)
    //         staminaBar.gameObject.SetActive(false);
    //     else if(stamina < maxStamina)
    //         staminaBar.gameObject.SetActive(true);
    // }
} 