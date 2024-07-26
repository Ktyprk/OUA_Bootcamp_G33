using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private GameObject player, playerCharacter, playerCamera, carCamera;
    [SerializeField] private PrometeoCarController carController;
    
    public bool canDrive = false;
    public bool isDriving = false;

    public ThirdPersonActionAsset playerActionAsset;
    private InputAction getInVehicle;

    public Transform playerpos;
    
    public ThirdPersonController thirdPersonController;
    private void Awake()
    {
        thirdPersonController = player.GetComponent<ThirdPersonController>();
    }
    

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (canDrive)
            {
                player.transform.position = playerpos.position;
                if (!isDriving)
                {
                    EnterVehicle();
                    isDriving = true;
                }
                else
                {
                    ExitVehicle();
                    isDriving = false;
                }
            }
        }

        if (isDriving)
        {
            player.transform.position = playerpos.position;
        }
    }
    

    private void EnterVehicle()
    {
        Debug.Log("Enter Vehicle");
        thirdPersonController.enabled = false; 
        carCamera.SetActive(true);
        playerCharacter.SetActive(false);
        carController.enabled = true; 
        player.transform.SetParent(this.transform);
    }
    
    private void ExitVehicle()
    {
        Debug.Log("Exit Vehicle");
        carCamera.SetActive(false);
        carController.enabled = false; 
        playerCharacter.SetActive(true);
        //player.transform.position = playerpos.position;
        player.transform.SetParent(null);

        thirdPersonController.enabled = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canDrive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isDriving)
        {
            canDrive = false;
        }
    }
}