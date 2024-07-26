using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;
using UnityEngine.AI;
using UnityEngine.UI;

public class InteractController : MonoBehaviour
{
    private Animator _animator;
    private ThirdPersonActionAsset playerActionAsset;
    private InputAction move, aim, fire, sprint , look, interact, inventory, closeUi;
    private bool isAim = false;
    private bool isAimOpen = false;
    private bool isUIopen = false;
    
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
        
    [SerializeField] private Camera playerCamera;

    private ISearchable iSearchable;
    private IItemCollectible iCollectible;
    private IItemAvailability iItemAvailability;
    private ISceneTransform iSceneTransform;
    private CharacterController _controller;
    
    public VehicleController vehicleController;
    public ThirdPersonController thirdPersonController;
    

    private void Awake()
    {
        playerActionAsset = new ThirdPersonActionAsset();
        interact = playerActionAsset.Player.Interact;
        inventory = playerActionAsset.Player.Inventory;
        aim = playerActionAsset.Player.Aim;
        fire = playerActionAsset.Player.Fire;
        closeUi = playerActionAsset.Player.CloseButton;
    }

    private void Start()
    {
     
    }

    private void OnEnable()
    {
        playerActionAsset.Enable();
        interact.performed += Interact;
        inventory.performed += Inventory;
        fire.performed += Fire;
        closeUi.performed += CloseInventory;
    }

    private void OnDisable()
    {
        playerActionAsset.Disable();
        interact.performed -= Interact;
        inventory.performed -= Inventory;
        fire.performed -= Fire;
        
    }
    
    private void Fire(InputAction.CallbackContext obj)
    {
        ItemScript equippedWeapon = InventoryManager.instance.GetEquippedWeapon();
        if (equippedWeapon != null)
        {
            if(isAim)
            equippedWeapon.Fire();
         
        }
    }

    private bool isdrive = false;
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     
        //             if (!isdrive)
        //             {
        //                 vehicleController.EnterVehicle();
        //                 isdrive = true;
        //             }
        //             else
        //             {
        //                 vehicleController.ExitVehicle();
        //                 isdrive = false;
        //             }
        //     
        //     
        // }
    }
        // if (!isAim && !isUIopen)
        // {
        //     Cursor.visible = false;
        //     Cursor.lockState = CursorLockMode.Locked;
        // }
        // // else if (isAim && !isUIopen)
        // // {
        // //     Cursor.visible = true;
        // //     Cursor.lockState = CursorLockMode.None;
        // //     LookAtMouse();
        // else if (isUIopen)
        // {
        //     Cursor.visible = true;
        //     Cursor.lockState = CursorLockMode.None;
        // }
    
    

    private void Interact(InputAction.CallbackContext context)
    {
        iCollectible?.Collect();
        iCollectible = null;
        iSearchable?.Search();
        iItemAvailability?.UseItem();
        iSceneTransform?.TransformScene();
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
        
        if (other.GetComponent<ISceneTransform>() != null)
        {
            iSceneTransform = other.GetComponent<ISceneTransform>();
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
        
        if (other.GetComponent<ISceneTransform>() != null && other.GetComponent<ISceneTransform>() == iSceneTransform)
        {
            iSceneTransform = null;
        }
    }
} 