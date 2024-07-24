using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    private Animator _animator;
    private ThirdPersonActionAsset playerActionAsset;
    private InputAction move, aim, fire, sprint, look, interact, inventory, closeUi;

    public bool isjump;
    public bool issprint;
    private bool isAim = false;
    private bool isUIopen = false;

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Camera playerCamera;

    private ISearchable iSearchable;
    private IItemCollectible iCollectible;
    private IItemAvailability iItemAvailability;
    private CharacterController _controller;

    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5.335f;
    public float SpeedChangeRate = 10.0f;

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

    private float _animationBlend;
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private int _animIDSetAim;

    private bool _hasAnimator;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

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
        interact.performed += Interact;
        inventory.performed += Inventory;
        fire.performed += Fire;
        aim.performed += OnAim;
        aim.canceled += OffAim;
    }

    private void OnDisable()
    {
        playerActionAsset.Disable();
        interact.performed -= Interact;
        inventory.performed -= Inventory;
        fire.performed -= Fire;
        aim.performed -= OnAim;
        aim.canceled -= OffAim;
    }

    private void OffAim(InputAction.CallbackContext obj)
    {
        isAim = false;
        _animator.SetBool(_animIDSetAim, isAim);
    }

    private void OnAim(InputAction.CallbackContext obj)
    {
        isAim = true;
        _animator.SetBool(_animIDSetAim, isAim);
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
            if (isAim)
                equippedWeapon.Fire();
        }
    }

    private void Update()
    {
        GroundedCheck();
        Movement();
        
        if (!isAim && !isUIopen)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            LookAt(move.ReadValue<Vector2>());
        }
        else if (isUIopen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Movement()
    {
        groundedPlayer = _controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(playerVelocity * Time.deltaTime);
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDSetAim = Animator.StringToHash("isPistolEquip");
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
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);
        }
    }

    private void LookAt(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            Vector3 forward = playerCamera.transform.forward;
            Vector3 right = playerCamera.transform.right;

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
}

   
