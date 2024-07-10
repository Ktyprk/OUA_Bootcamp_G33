using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    
    public static InventoryManager instance;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public EquipSlotManager equipSlotManager;
    
    public ThirdPersonActionAsset playerActionAsset;
    private InputAction UIinput, UIcancel;
    private int selectedSlot = 0;
    private const int maxSlots = 28;
    private ItemScript equipedItemScript;

    public GameObject mainInventory;

    private Item equippedWeapon;
    private bool isInventoryOpen = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        playerActionAsset = new ThirdPersonActionAsset();
        UIinput = playerActionAsset.UI.InGameNavigate;
        UIcancel = playerActionAsset.UI.Cancel;
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        for (int i = 1; i <= 5; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                ChangeSelectedSlot(i - 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RemoveItem(0, 1);
        }
    }

    public void OpenInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        if(isInventoryOpen)
        {
            mainInventory.SetActive(true);
        }
        else
        {
            mainInventory.SetActive(false);
        }
    }
    
    public void CloseInventory()
    {
        mainInventory.SetActive(false);
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
        if (inventorySlots.Length > 0)
        {
            if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
            {
                inventorySlots[selectedSlot]?.Deselect();
            }

            selectedSlot = (newSlot + maxSlots) % maxSlots;

            if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
            {
                inventorySlots[selectedSlot]?.Select();
            }
        }
    }

    public bool AddItem(Item item, ItemScript itemScript)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
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

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                if (item.itemPrefab != null)
                {
                    GameObject script = Instantiate(item.itemPrefab, slot.transform);
                
                    SpawnNewItem(item, slot, script.GetComponent<ItemScript>());
                }
                else
                {
                    SpawnNewItem(item, slot, null);
                }
               
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
        return itemInSlot?.item;
    }
    
    public ItemScript GetSelectedItemSlot()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        Debug.Log(itemInSlot.itemScript == null);
        return itemInSlot.itemScript;
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

    public void UnequipItem(Item item)
    {
        if (item == equippedWeapon)
        {
            equipSlotManager.UnequipItem(EquipSlotManager.EquipType.Weapon);
            equippedWeapon = null;
            equipedItemScript = null;
        }
        else
        {
            Debug.LogWarning("Belirtilen eşya zaten takılı değil: " + item.itemName);
        }
    }

    public void SetEquippedWeapon(Item item)
    {
        equippedWeapon = item;
        equipSlotManager.EquipItem(item, GetSelectedItemSlot(), EquipSlotManager.EquipType.Weapon);
       equipedItemScript = GetSelectedItemSlot();
    }

    public ItemScript GetEquippedWeapon()
    {
        return equipedItemScript;
    }
    
    public bool IsItemEquipped(Item item)
    {
        return item == equippedWeapon;
    }
    
   
    
}
