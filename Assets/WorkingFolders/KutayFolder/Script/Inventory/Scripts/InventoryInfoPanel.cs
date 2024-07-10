using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryInfoPanel : MonoBehaviour
{
    private Item selectedItem;

    public GameObject player;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI actionButtonText;

    void Update()
    {
        if (selectedItem != InventoryManager.instance.GetSelectedItem())
        {
            UpdateSelectedItem();
        }
    }
    
    public void UpdateSelectedItem()
    {
        selectedItem = InventoryManager.instance.GetSelectedItem();
        
        if (selectedItem != null)
        {
            itemNameText.text = selectedItem.itemName;
            itemDescriptionText.text = selectedItem.itemDescription;
            itemImage.sprite = selectedItem.itemIcon; 
            SetActionButton();
        }
        else
        {
            itemNameText.text = "Seçili öğe yok";
            itemDescriptionText.text = "";
            itemImage.sprite = null; 
            actionButtonText.text = "";
        }
    }
    
    public void SetActionButton()
    {
        if(selectedItem.isuseable)
        {
            actionButtonText.text = "Kullan";
        }
        else if(selectedItem.isequippable)
        {
            if (IsItemEquipped(selectedItem))
            {
                actionButtonText.text = "Unequip";
            }
            else
            {
                actionButtonText.text = "Equip";
            }
        }
    }

    private bool IsItemEquipped(Item item)
    {
        if (InventoryManager.instance.IsItemEquipped(item))
        {
            return true;
        }
        return false;
    }
    
    public void OnActionButtonClick()
    {
        if (selectedItem != null)
        {
            if(selectedItem.isuseable)
            {
                InventoryManager.instance.UseSelectedItem(selectedItem.itemID, true);
            }
            else if(selectedItem.isequippable)
            {
                ToggleEquipItem(selectedItem);
            }
        }
    }
    
    private void ToggleEquipItem(Item item)
    {
        if (IsItemEquipped(item))
        {
            UnequipItem(item);
        }
        else
        {
            EquipItem(item);
        }
    }
    
    private void EquipItem(Item item)
    {
        InventoryManager.instance.SetEquippedWeapon(item);
        SetActionButton();
    }
    
    private void UnequipItem(Item item)
    {
        InventoryManager.instance.UnequipItem(item);
        SetActionButton(); 
        itemImage.sprite = null; 
    }
    
    public void OnDropButtonClick()
    {
        if (selectedItem != null)
        {
            Item item = selectedItem;
            Instantiate(item.itemPrefab, player.transform.position, Quaternion.identity);
            InventoryManager.instance.RemoveItem(item.itemID, 1);
        }
    }
}
