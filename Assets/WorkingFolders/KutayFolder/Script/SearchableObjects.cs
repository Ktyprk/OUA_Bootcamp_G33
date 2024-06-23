using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SearchableObjects : MonoBehaviour, ISearchable
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float searchTime = 2f;

    private bool hasBeenSearched = false;
    
    [SerializeField] private GameObject buttonPrefab; 
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private List<Item> items = new List<Item>();
     private List<LootController> lootControllers = new List<LootController>();
     private int selectedLootIndex = 0;
     
     public ThirdPersonActionAsset playerActionAsset;
     private InputAction UIinput;
     
     private void Awake()
     {
         playerActionAsset = new ThirdPersonActionAsset();
         UIinput = playerActionAsset.UI.InGameNavigate;
     }
     
    public void Search()
    {
        if (!hasBeenSearched)
        {
            StopAllCoroutines();
            StartCoroutine(SearchObject());
        }
        else
        {
           //true
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
           
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(lootControllers[selectedLootIndex].item.itemID);
            items.Remove(lootControllers[selectedLootIndex].item);
            Destroy(lootControllers[selectedLootIndex].gameObject);
            OpenList();
        }
    }
    
    private void OnEnable()
    {
        playerActionAsset.Enable();
        UIinput.performed += listNavigation;
    }

    private void OnDisable()
    {
        playerActionAsset.Disable();
        UIinput.performed -= listNavigation;
    }
    
    private void listNavigation(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();

        if (inputVector.y > 0)
        {
            lootControllers[selectedLootIndex].ToggleHighlight(false);
            selectedLootIndex--;
            if(selectedLootIndex < 0)
                selectedLootIndex = lootControllers.Count - 1;
            lootControllers[selectedLootIndex].ToggleHighlight(true);
            
        }
        else if (inputVector.y < 0)
        {
            lootControllers[selectedLootIndex].ToggleHighlight(false);
            selectedLootIndex++;
            if(selectedLootIndex >= lootControllers.Count)
                selectedLootIndex = 0;
            
            lootControllers[selectedLootIndex].ToggleHighlight(true);
        }
        
    }
    
    private IEnumerator SearchObject()
    {
        float elapsedTime = 0f;
        fillImage.fillAmount = 0f; 

        while (elapsedTime < searchTime)
        {
            elapsedTime += Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(elapsedTime / searchTime);
            yield return null;
        }

        fillImage.fillAmount = 1f; 
        hasBeenSearched = true; 
        fillImage.gameObject.SetActive(false);
        OpenList();
    }
    
    private void OpenList()
    {
        foreach (LootController lootController in lootControllers)
        {
            Destroy(lootController.gameObject);
        }
        
        lootControllers.Clear();
        foreach (Item item in items)
        {
            GameObject buttonObject = Instantiate(buttonPrefab, scrollViewContent);
            LootController lootController = buttonObject.GetComponent<LootController>();

            if (lootController != null)
            {
                lootControllers.Add(lootController);
                lootController.SetItemDetails(item);
            }
        }
    }
    
    
    
}
