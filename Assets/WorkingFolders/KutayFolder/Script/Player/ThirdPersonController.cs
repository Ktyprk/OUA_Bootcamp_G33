using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.AI;

public class ThirdPersonController : MonoBehaviour
{
    private ThirdPersonActionAsset playerActionAsset;
    private InputAction move, aim, fire, sprint, look, interact, inventory, closeUi;
    
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    private ISearchable iSearchable;
    private IItemCollectible iCollectible;
    private IItemAvailability iItemAvailability;
    private CharacterController _controller;
    private VehicleController _Vcontroller;
    
    private float _speed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5.335f;
    public float SpeedChangeRate = 10.0f;
    
    public bool isSprinting = false;
    private bool isAim = false;
    private bool isUIopen = false;

    private void Awake()
    {
        playerActionAsset = new ThirdPersonActionAsset();
        move = playerActionAsset.Player.Move;
        sprint = playerActionAsset.Player.Sprint;
        look = playerActionAsset.Player.Look;
        interact = playerActionAsset.Player.Interact;
        inventory = playerActionAsset.Player.Inventory;
        aim = playerActionAsset.Player.Aim;
        fire = playerActionAsset.Player.Fire;
        closeUi = playerActionAsset.Player.CloseButton;
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerActionAsset.Enable();
        look.performed += Look;
        look.canceled += Look;
        interact.performed += Interact;
        inventory.performed += Inventory;
        sprint.performed += Sprint;
        sprint.canceled += Sprint;
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
        sprint.performed -= Sprint;
        sprint.canceled -= Sprint;
        aim.performed -= Aim;
        aim.canceled -= StopAiming;
        fire.performed -= Fire;
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
            Debug.Log("No weapon equipped");
        }
    }

    private void Update()
    {
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
    
    private void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }

    [SerializeField] private float gravity = 9.81f;

    private void Move()
    {
        Vector2 input = move.ReadValue<Vector2>();
        
        float targetSpeed = isSprinting ? SprintSpeed : MoveSpeed;
        
        if (input == Vector2.zero && !isSprinting)
        {
            targetSpeed = 0.0f;
        }
        else if (input != Vector2.zero && !isSprinting)
        {
            targetSpeed = MoveSpeed;
        }
        
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * input.y + right * input.x;
        moveDirection.Normalize();
        
        if (!_controller.isGrounded)
        {
            moveDirection += Vector3.down * gravity;
        }
        
        _speed = Mathf.Lerp(_speed, targetSpeed, Time.deltaTime * SpeedChangeRate);
        
        _controller.Move(moveDirection * _speed * Time.deltaTime);
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
        isUIopen = false;
        InventoryManager.instance.CloseInventory();
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
}
