using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private GameObject player, playerCharacter, carCamera;
    [SerializeField] private PrometeoCarController carController;
    
    public bool canDrive = false;
    public bool isDriving = false;

    private ThirdPersonActionAsset playerActionAsset;
    private InputAction getInVehicle;

    public Transform playerpos;
    private void Awake()
    {
        playerActionAsset = new ThirdPersonActionAsset();
        getInVehicle = playerActionAsset.Player.Vehicle;
    }

    private void OnEnable()
    {
        playerActionAsset.Enable();
        getInVehicle.performed += ToggleDrive;
    }

    private void OnDisable()
    {
        playerActionAsset.Disable();
        getInVehicle.performed -= ToggleDrive;
    }

    private void Update()
    {
        if (isDriving)
        {
            
        }
    }

    private void ToggleDrive(InputAction.CallbackContext context)
    {
        if (isDriving)
        {
            ExitVehicle();
        }
        else if(canDrive)
        {
            EnterVehicle();
        }
        isDriving = !isDriving;
    }

    private void EnterVehicle()
    {
        player.GetComponent<ThirdPersonController>().enabled = false;
        carCamera.SetActive(true);
        playerCharacter.SetActive(false);
        carController.enabled = true;
        player.transform.SetParent(this.transform);
    }

    private void ExitVehicle()
    {
        
        player.transform.position = playerpos.position;
        carCamera.SetActive(false);
        playerCharacter.SetActive(true);
        carController.enabled = false;
        player.transform.SetParent(null, true);
        canDrive = false;
        StopCoroutine(ExitVehicleCoroutine());
        StartCoroutine(ExitVehicleCoroutine());
    }
    
    IEnumerator ExitVehicleCoroutine()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        player.GetComponent<ThirdPersonController>().enabled = true;
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
        if (other.CompareTag("Player"))
        {
            canDrive = false;
        }
    }
}
