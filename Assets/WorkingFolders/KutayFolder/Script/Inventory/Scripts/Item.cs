using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
    
    public GameObject itemPrefab;
    
    public enum ItemType
    {
        Weapon,
        Consumable,
        Quest,
        Money
    }
}