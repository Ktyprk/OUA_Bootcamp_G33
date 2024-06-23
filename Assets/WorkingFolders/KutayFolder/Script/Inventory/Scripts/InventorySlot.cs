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
        if(transform.childCount == 0)
        {
            InventoryItem inventoryitem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryitem.parentAfterDrag = transform;
        }
    }
}
