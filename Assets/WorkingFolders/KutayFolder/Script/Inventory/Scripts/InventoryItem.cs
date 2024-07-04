using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{ 
    
    [Header("UI")] 
    public Image image;
    public TMP_Text countText;
   
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
     public Transform parentBeforeDrag, parentAfterDrag;
   
    public ItemScript itemScript;
   
    public void InitializeItem(Item newItem, ItemScript itemScript)
    {
        this.itemScript = itemScript;
        item = newItem;
        image.sprite = newItem.itemIcon;
        RefreshCount();
    }
   
    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
        bool textDective = count < 1;
        if(textDective)
            countText.gameObject.SetActive(false);
    }
   
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        parentBeforeDrag = transform.parent;
        transform.SetParent(transform.root);
    }
   
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
   
    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
    
    
}