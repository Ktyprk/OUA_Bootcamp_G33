using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoItemAvailability : MonoBehaviour, IItemAvailability
{
    [SerializeField] private int _usableItemId;
    [SerializeField] private int _usableItemCount; 

    public int usableItemId 
    { 
        get { return _usableItemId; } 
        set { _usableItemId = value; } 
    }

    public int usableItemCount 
    { 
        get { return _usableItemCount; } 
        set { _usableItemCount = value; } 
    }

    public GameObject itemUseInfoPrefab;
    
    public void UseItem()
    { 
        InventoryManager.instance.RemoveItem(usableItemId, usableItemCount); 
 
        // Item item = InventoryManager.instance.GetSelectedItem();
        //
        // if (item.itemID == usableitemid)
        // {
        //     InventoryManager.instance.UseSelectedItem(item.itemID, true);
        //     OnItemUsed?.Invoke();
        // }
        
    
        
    }
}