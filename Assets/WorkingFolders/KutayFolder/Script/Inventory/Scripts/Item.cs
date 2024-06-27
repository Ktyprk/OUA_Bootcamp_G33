using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int itemID;
    public string itemDescription;
    public int itemValue;
    public int itemDamage;
    public int itemArmor;
    public ItemType itemType;
    
    public bool stackable, isuseable, isequippable;
    public int maxStackSize = 4;
    
    private string itemUseText = "Use";
    private string itemEquipText = "Equip";
    
    public GameObject itemPrefab;
    
    public enum ItemType
    {
        Weapon,
        Consumable,
        Quest,
        Money
    }
    
    public Item(string name, Sprite icon, int id, string description, int value, int damage, int armor, ItemType type)
    {
        itemName = name;
        itemIcon = icon;
        itemID = id;
        itemDescription = description;
        itemValue = value;
        itemDamage = damage;
        itemArmor = armor;
        itemType = type;
    }
    
    // public Item(Item item)
    // {
    //     itemName = item.itemName;
    //     itemIcon = item.itemIcon;
    //     itemID = item.itemID;
    //     itemDescription = item.itemDescription;
    //     itemValue = item.itemValue;
    //     itemDamage = item.itemDamage;
    //     itemArmor = item.itemArmor;
    //     itemType = item.itemType;
    // }
    
}
