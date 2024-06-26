using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.AI;

public class ThirdPersonController : MonoBehaviour
{
    private ThirdPersonActionAsset playerActionAsset;
    private InputAction move, look, interact, inventory;
    
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    private ISearchable iSearchable;
    private ICollectible iCollectible;
    private IItemAvailability iItemAvailability;

    private void Awake()
    {
        playerActionAsset = new ThirdPersonActionAsset();
        move = playerActionAsset.Player.Move;
        look = playerActionAsset.Player.Look;
        interact = playerActionAsset.Player.Interact;
        inventory = playerActionAsset.Player.Inventory;
    }

    private void OnEnable()
    {
        playerActionAsset.Enable();
        look.performed += Look;
        look.canceled += Look;
        interact.performed += Interact;
        inventory.performed += Inventory;
    }

    private void OnDisable()
    {
        playerActionAsset.Disable();
        look.performed -= Look;
        look.canceled -= Look;
        interact.performed -= Interact;
        inventory.performed -= Inventory;
    }

    private void Update()
    {
        Vector2 input = move.ReadValue<Vector2>();
        
        LookAt(input);

        Vector3 forward = freeLookCamera.transform.forward;
        Vector3 right = freeLookCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
    
        Vector3 moveDirection = forward * input.y + right * input.x;
        moveDirection.Normalize();
        
        transform.Translate(moveDirection * maxSpeed * Time.deltaTime, Space.World);
        
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

            // Calculate the rotation towards the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Look(InputAction.CallbackContext context)
    {
        // if (context.performed)
        // {
        //     freeLookCamera.m_XAxis.m_MaxSpeed = 300f;
        // }
        // else if (context.canceled)
        // {
        //     freeLookCamera.m_XAxis.m_MaxSpeed = 0f;
        // }
    }
    
    private void Interact(InputAction.CallbackContext context)
    {
        iSearchable?.Search();
        iCollectible?.Collect();
        iItemAvailability?.UseItem();
    }
    
    private void Inventory(InputAction.CallbackContext context)
    {
        InventoryManager.instance.OpenInventory();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ISearchable>() != null)
        {
            iSearchable = other.GetComponent<ISearchable>();
        }
        
        if (other.GetComponent<ICollectible>() != null)
        {
            iCollectible = other.GetComponent<ICollectible>();
        }
          
        if (other.GetComponent<IItemAvailability>() != null)
        {
            iItemAvailability = other.GetComponent<IItemAvailability>();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ISearchable>() == iSearchable)
        {
            iSearchable = null;
        }
        
        if (other.GetComponent<ICollectible>() == iCollectible)
        {
            iCollectible = null;
        }
        
        if (other.GetComponent<IItemAvailability>() == iItemAvailability)
        {
            iItemAvailability = null;
        }
    }
}
