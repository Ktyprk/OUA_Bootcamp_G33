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
        // Only update the item info if the selected item has changed
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
            itemImage.sprite = selectedItem.itemIcon; // Assuming itemIcon is a sprite in the Item class
            SetActionButton();
        }
        else
        {
            itemNameText.text = "No item selected";
            itemDescriptionText.text = "";
            itemImage.sprite = null; // Clear the image
        }
    }
    
    public void SetActionButton()
    {
        if(selectedItem.isuseable)
        {
            actionButtonText.text = "Use";
        }
        else if(selectedItem.isequippable)
        {
            actionButtonText.text = "Equip";
        }
    }
    
    public void OnActionButtonClick()
    {
            Item item = selectedItem;
            Instantiate(item.itemPrefab, player.transform.position, Quaternion.identity);
            InventoryManager.instance.RemoveItem(item.itemID, 1);
    }
}