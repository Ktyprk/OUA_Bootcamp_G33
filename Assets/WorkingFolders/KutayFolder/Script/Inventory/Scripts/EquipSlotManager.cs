using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquipSlotManager : MonoBehaviour
{
    public enum EquipType
    {
        Weapon,
    }
    
    public EquipSlot weaponSlot;
    
    public void EquipItem(Item item, ItemScript itemscript, EquipType equipType)
    {
        switch (equipType)
        {
            case EquipType.Weapon:
                weaponSlot.Equip(item, weaponSlot.itemImage);
                break;
            default:
                Debug.LogWarning("EquipType not recognized");
                break;
        }
    }
    
    public void UnequipItem(EquipType equipType)
    {
        switch (equipType)
        {
            case EquipType.Weapon:
                weaponSlot.Unequip();
                break;
            default:
                Debug.LogWarning("EquipType not recognized");
                break;
        }
    }
}


[System.Serializable]
public class EquipSlot
{
    public Item currentItem;
    public Image itemImage; 

    public void Equip(Item item, Image newItemImage)
    {
        currentItem = item;
        itemImage = newItemImage;
        UpdateItemImage();
        Debug.Log("Equipped: " + item.itemName);
    }

    public void Unequip()
    {
        currentItem = null;
        itemImage.sprite = null; 
        itemImage = null;
    }

    private void UpdateItemImage()
    {
        if (itemImage != null && currentItem != null)
        {
            itemImage.sprite = currentItem.itemIcon; 
        }
    }
}
