using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance;
    [SerializeField] private Item[] items;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    public int GetItemIndex(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                return i;
            }
        }
        return -1;
    }
    
    public Item GetItem(int index)
    {
        if (index == -1)
        {
            return null;
        }
        return items[index];
    }
}
