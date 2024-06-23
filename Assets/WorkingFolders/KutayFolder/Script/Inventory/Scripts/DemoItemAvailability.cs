using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoItemAvailability : MonoBehaviour, IItemAvailability
{
    public int usableitemid;
    
    
    public void UseItem()
    {
        InventoryManager.instance.GetSelectedItem(usableitemid,true);
    }
    
}
