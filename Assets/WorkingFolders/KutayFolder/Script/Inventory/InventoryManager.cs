using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<Item> items = new List<Item>();
    public int space = 20;
    public delegate void OnItemChanged();       
    public OnItemChanged onItemChangedCallback;

    //public InventoryItemController[] InventoryItems;
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }
    
    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Not enough room.");
            return false;
        }
        items.Add(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        } 
        return true;
    }
    
    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
    
}
