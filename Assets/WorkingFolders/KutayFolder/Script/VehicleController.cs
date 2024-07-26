using System;
using System.Collections;
using System.Collections.Generic;
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
        if (canDrive)
        {
            Debug.Log("Can Drive");
            // if (!isDriving)
            // {
            //     EnterVehicle();
            //     isDriving = true;
            // }
            // else
            // {
            //     ExitVehicle();
            //     isDriving = false;
            // }
        }
        
    }

    // private void EnterVehicle()
    // {
    //     carCamera.SetActive(true);
    //     playerCamera.SetActive(false);
    //     playerCharacter.SetActive(false);
    //     carController.enabled = true;
    //     player.transform.SetParent(this.transform);
    // }
    //
    // private void ExitVehicle()
    // {
    //     
    //     player.transform.position = playerpos.position;
    //     playerCamera.SetActive(true);
    //     //thirdPersonController.enabled = true;
    //     carCamera.SetActive(false);
    //     playerCharacter.SetActive(true);
    //     carController.enabled = false;
    //     player.transform.SetParent(null, true);
    //     canDrive = false;
    //     StopCoroutine(ExitVehicleCoroutine());
    //     StartCoroutine(ExitVehicleCoroutine());
    // }
    //
    // IEnumerator ExitVehicleCoroutine()
    // {
    //     yield return new WaitForSeconds(Time.deltaTime);
    //     player.GetComponent<ThirdPersonController>().enabled = true;
    // }

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
