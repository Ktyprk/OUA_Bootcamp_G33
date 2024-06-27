using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        image.color = selectedColor;
    }
    
    public void Deselect()
    {
        image.color = notSelectedColor;
    }
    
    
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (droppedItem != null)
        {
            if (transform.childCount == 0)
            {
                InventoryItem inventoryitem = eventData.pointerDrag.GetComponent<InventoryItem>(); 
                inventoryitem.parentAfterDrag = transform;
            }
            else
            {
                InventoryItem currentSlotItem = transform.GetChild(0).GetComponent<InventoryItem>();
                
                Transform droppedItemParentBeforeDrag = droppedItem.parentBeforeDrag;
                Transform currentSlotItemParentBeforeDrag = currentSlotItem.transform.parent;
                
                droppedItem.transform.SetParent(transform);
                currentSlotItem.transform.SetParent(droppedItemParentBeforeDrag);
                
                droppedItem.parentAfterDrag = transform;
                currentSlotItem.parentBeforeDrag = droppedItemParentBeforeDrag;
                
                currentSlotItemParentBeforeDrag.GetComponent<InventorySlot>().Deselect();
                Deselect();
            }
        }
    }


    
}
