using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;
    
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     PickupItem(0);
        // }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //UseSelectedItem();
        }
    }
    
    public void PickupItem(int id)
    {
        bool result = inventoryManager.AddItem(itemsToPickup[id], null);
    }
    //
    // public void GetSelectedItem()
    // {
    //     Item recivedItem = inventoryManager.GetSelectedItem();
    //     if(recivedItem != null)
    //     {
    //         Debug.Log("Selected item is: " + recivedItem.itemName);
    //     }
    //     else
    //     {
    //         Debug.Log("No item selected");
    //     }
    // }
    
    
    
    // public void UseSelectedItem()
    // {
    //     Item recivedItem = inventoryManager.GetSelectedItem(true);
    //     if(recivedItem != null)
    //     {
    //         Debug.Log("Used item: " + recivedItem.itemName);
    //     }
    //     else
    //     {
    //         Debug.Log("No item used");
    //     }
    // }
}
