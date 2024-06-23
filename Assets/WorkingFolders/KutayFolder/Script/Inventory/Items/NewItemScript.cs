using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewItemScript : MonoBehaviour, ICollectible
{
    public ScriptableObject itemsc;
    
    public void Collect()
    {
        PickupItem();
    }
    
    public void PickupItem()
    {
        Item item = (Item)itemsc;
        bool result = InventoryManager.instance.AddItem(item);
    }
}
