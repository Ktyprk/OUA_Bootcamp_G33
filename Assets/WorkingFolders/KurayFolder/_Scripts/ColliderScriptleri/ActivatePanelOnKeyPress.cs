using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePanelOnKeyPress : MonoBehaviour
{
    public string playerTag = "Player";
    public GameObject panelToActivate;  // Panel referans�

    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            
            Debug.Log("F tu�una bas�ld� ve panel a��l�yor.");
            panelToActivate.SetActive(true);  // Paneli aktif hale getir
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(playerTag))
        {
            Debug.Log("Oyuncu collidera girdi.");
            isPlayerNearby = true;  // Oyuncu yak�n oldu�unda
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Oyuncu colliderdan ��kt�.");
            isPlayerNearby = false;  // Oyuncu uzakla�t���nda
        }
    }
}