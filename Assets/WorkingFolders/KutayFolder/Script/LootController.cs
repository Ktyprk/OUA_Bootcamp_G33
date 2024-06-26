using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootController : MonoBehaviour
{
    [SerializeField] private GameObject highlightedEffect;
    
    public Image lootImage;
    public TextMeshProUGUI lootNameText;
    public TextMeshProUGUI lootText;
    public Image missionImage;
    public Item item;
    
    public void SetItemDetails(Item item)
    {
        this.item = item;
        lootImage.sprite = item.itemIcon;
        lootNameText.text = item.itemName;
        lootText.text = item.itemDescription;
        
    }
    
    public void ToggleHighlight(bool toggle)
    {
        highlightedEffect.SetActive(toggle);
    }
}
