using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePanelOnKeyPress : MonoBehaviour
{
    public string playerTag = "Player";
    public GameObject panelToActivate;  // Panel referansý

    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            
            Debug.Log("F tuþuna basýldý ve panel açýlýyor.");
            panelToActivate.SetActive(true);  // Paneli aktif hale getir
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(playerTag))
        {
            Debug.Log("Oyuncu collidera girdi.");
            isPlayerNearby = true;  // Oyuncu yakýn olduðunda
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Oyuncu colliderdan çýktý.");
            isPlayerNearby = false;  // Oyuncu uzaklaþtýðýnda
        }
    }
}