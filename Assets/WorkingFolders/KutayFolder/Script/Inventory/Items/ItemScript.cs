using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour, IItemCollectible
{
    public ScriptableObject itemsc;
    
    public void Collect()
    {
        PickupItem();
    }

    public void PickupItem()
    {
        Item item = (Item)itemsc;
        bool result = InventoryManager.instance.AddItem(item, this);
        Destroy(gameObject);
        
    }
    
    public virtual void InteractItem()
    {
        Debug.Log("Interacting with item");
    }

}

public interface IItemCollectible 
{ 
    public void Collect();
}

