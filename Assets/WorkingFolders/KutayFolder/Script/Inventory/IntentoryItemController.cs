using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntentoryItemController : MonoBehaviour
{
     Item item;
     
     public Button removeButton;
     
     public void RemoveItem()
     {
         InventoryManager.instance.Remove(item);
     }
     
     public void AddItem(Item newItem)
     {
         item = newItem;
     }
   
}
