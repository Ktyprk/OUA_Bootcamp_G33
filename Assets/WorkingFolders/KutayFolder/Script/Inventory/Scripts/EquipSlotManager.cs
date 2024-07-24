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
    public Transform weaponSpawnPoint;

    private Transform playerTransform;
    private GameObject instantiatedItem;

    private void Start()
    {
        // Find the player's transform using the tag "Player"
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (SaveSystem.Instance.equippedItem > -1)
        {
            equippedItem = SaveSystem.Instance.equippedItem;
            Item item = ItemDataBase.instance.GetItem(equippedItem);
            ItemScript itemscript = InventoryManager.instance.GetItem(equippedItem);
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
                weaponSlot.Equip(item);
                instantiatedItem = Instantiate(item.itemPrefab, weaponSpawnPoint.position, weaponSpawnPoint.rotation);
                instantiatedItem.transform.SetParent(weaponSpawnPoint, false);
                instantiatedItem.transform.localPosition = Vector3.zero;
                instantiatedItem.transform.localRotation = weaponSpawnPoint.rotation;
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
                if (instantiatedItem != null)
                {
                    Destroy(instantiatedItem);
                    instantiatedItem = null;
                }
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

    public void Equip(Item item)
    {
        currentItem = item;
        UpdateItemImage();
        Debug.Log("Equipped: " + item.itemName);
    }

    public void Unequip()
    {
        currentItem = null;
        UpdateItemImage();
        Debug.Log("Unequipped");
    }

    private void UpdateItemImage()
    {
        if (itemImage != null)
        {
            if (currentItem != null)
            {
                itemImage.sprite = currentItem.itemIcon;
            }
            else
            {
                itemImage.sprite = null;
            }
        }
    }
}