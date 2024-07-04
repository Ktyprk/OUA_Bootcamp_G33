using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoItemAvailability : MonoBehaviour, IItemAvailability
{
    public int usableitemid;
    
    public System.Action OnItemUsed;

    public void UseItem()
    {
        Item item = InventoryManager.instance.GetSelectedItem();
        
        if (item.itemID == usableitemid)
        {
            InventoryManager.instance.UseSelectedItem(item.itemID, true);
            OnItemUsed?.Invoke();
        }
    
        
    }
}