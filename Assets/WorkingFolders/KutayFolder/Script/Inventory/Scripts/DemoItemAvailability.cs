using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoItemAvailability : MonoBehaviour, IItemAvailability
{
    public int usableitemid;

    public void UseItem()
    {
        Item item = InventoryManager.instance.GetSelectedItem();
        
        try 
        {
            if (item.itemID == usableitemid)
            {
                InventoryManager.instance.UseSelectedItem(item.itemID, true);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error using item: " + ex.Message);
        }
        
    }
}