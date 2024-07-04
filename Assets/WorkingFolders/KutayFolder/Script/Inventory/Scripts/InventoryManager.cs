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
    private const int maxSlots = 28;
    
    public GameObject mainInventory;

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
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RemoveItem(0, 1);
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
    
    public bool AddItem(Item item, ItemScript itemScript)
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
                SpawnNewItem(item, slot, itemScript);
                return true;
            }
        }
        
        return false;
    }
    
    private void SpawnNewItem(Item item, InventorySlot slot, ItemScript itemScript)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item, itemScript);
    }
    
    public Item GetSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            int itemId = itemInSlot.item.itemID; 
            Item item = itemInSlot.item;
            
            if (itemInSlot.count > 0)
            {
               Debug.Log(item);
                
            }
            return itemInSlot.item;
        }
        return null;
    }
    
    public Item UseSelectedItem(int itemid, bool use)
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
                itemInSlot.itemScript.InteractItem();
                if (itemInSlot.count == 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
                return item; 
            }
            else
            {
                return null;
            }
        }
    
        return null; 
    }

    
    public bool RemoveItem(int itemID, int amount)
    {
        for (int i = inventorySlots.Length - 1; i >= 0; i--)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item.itemID == itemID)
            {
                if (itemInSlot.count >= amount)
                {
                    itemInSlot.count -= amount;
                    if (itemInSlot.count <= 0)
                    {
                        Destroy(itemInSlot.gameObject);
                    }
                    else
                    {
                        itemInSlot.RefreshCount();
                    }
                    return true;
                }
            }
        }
        return false;
    }

    
}
