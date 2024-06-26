using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    
    public ThirdPersonActionAsset playerActionAsset;
    private InputAction UIinput;
    private int selectedSlot = 0;
    private const int maxSlots = 4;
    
    public GameObject mainInventory;
    public ScriptableObject itemsc;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        playerActionAsset = new ThirdPersonActionAsset();
        UIinput = playerActionAsset.UI.InGameNavigate;
       
    }
    
    private void Start()
    {
        ChangeSelectedSlot(0);
    }
    
    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number > 0 && number <= 5)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }
    
    public void OpenInventory()
    {
        if(mainInventory.activeSelf == true)
        {
            mainInventory.SetActive(false);
        }
        else
        {
            mainInventory.SetActive(true);
        }
    }
    
    private void OnEnable()
    {
        playerActionAsset.Enable();
        UIinput.performed += uiNavigation;
    }

    private void OnDisable()
    {
        playerActionAsset.Disable();
        UIinput.performed -= uiNavigation;
    }

    private void uiNavigation(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();

        if (inputVector.x > 0)
        {
            ChangeSelectedSlot(selectedSlot + 1);
            
        }
        else if (inputVector.x < 0)
        {
            ChangeSelectedSlot(selectedSlot - 1);
        }
        
    }

    void ChangeSelectedSlot(int newSlot)
    {
        if (inventorySlots != null && inventorySlots.Length > 0)
        {
            if (selectedSlot >= 0)
            {
                inventorySlots[selectedSlot].Deselect();
            }
            
            if (newSlot >= maxSlots)
            {
                selectedSlot = 0;
            }
            else if (newSlot < 0)
            {
                selectedSlot = maxSlots - 1;
            }
            else
            {
                selectedSlot = newSlot;
            }

            inventorySlots[selectedSlot].Select();
        }

    }
    
    public bool AddItem(Item item)
    {
        for (int i = 0; i <inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && item.stackable && itemInSlot.count < item.maxStackSize)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        
        for (int i = 0; i <inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        
        return false;
    }
    
    private void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }
    
    public Item GetSelectedItem(int itemid, bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            int itemId = itemInSlot.item.itemID; 
            Item item = itemInSlot.item;
            
            if (itemId == itemid && use)
            {  
                itemInSlot.count--;
                if(itemInSlot.count == 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
                
            }
            return itemInSlot.item;
        }
        return null;
    }
    
    // public bool RemoveItem(Item item)
    // {
    //     // Check if the item is stackable and decrement the count if possible
    //     for (int i = 0; i < inventorySlots.Length; i++)
    //     {
    //         InventorySlot slot = inventorySlots[i];
    //         InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
    //         if (itemInSlot != null && itemInSlot.item == item && item.stackable && itemInSlot.count > 0)
    //         {
    //             itemInSlot.count--;
    //             itemInSlot.RefreshCount();
    //         
    //             // If count reaches zero, remove the item from the slot
    //             if (itemInSlot.count == 0)
    //             {
    //                 Destroy(itemInSlot.gameObject);
    //             }
    //             return true;
    //         }
    //     }
    //
    //     // If the item is not stackable, search for and remove it from an occupied slot
    //     for (int i = 0; i < inventorySlots.Length; i++)
    //     {
    //         InventorySlot slot = inventorySlots[i];
    //         InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
    //         if (itemInSlot != null && itemInSlot.item == item && !item.stackable)
    //         {
    //             Destroy(itemInSlot.gameObject);
    //             return true;
    //         }
    //     }
    //
    //     // Item was not found in the inventory
    //     return false;
    // }

}
