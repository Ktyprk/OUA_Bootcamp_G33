using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquipSlotManager : MonoBehaviour
{
    private int equippedItem;
    public enum EquipType
    {
        Weapon,
    }
    
    public EquipSlot weaponSlot;
    
    private void Start()
    {
        if (SaveSystem.Instance.equippedItem > -1)
        {
            equippedItem = SaveSystem.Instance.equippedItem;
            Item item = ItemDataBase.instance.GetItem(equippedItem);
            ItemScript itemscript = Instantiate(item.itemPrefab).GetComponent<ItemScript>();
            EquipItem(item, itemscript, EquipType.Weapon);
            InventoryManager.instance.SetCustomEquippedWeapon(item, itemscript);
        }
    }
    
    public void EquipItem(Item item, ItemScript itemscript, EquipType equipType)
    {
        equippedItem = ItemDataBase.instance.GetItemIndex(item);
        Save();
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
        equippedItem = -1;
        Save();
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

    public void Save()
    {
        SaveSystem.Instance.equippedItem = equippedItem;
        SaveSystem.Save();
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
