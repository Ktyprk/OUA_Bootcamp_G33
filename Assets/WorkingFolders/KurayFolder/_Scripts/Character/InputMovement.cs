using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.AI;

public class InputMovement2 : MonoBehaviour
{
    public CameraManager cameraManager;
    private NightDetective2 playerActionAsset;
    private InputAction move, fire;
    public NavMeshAgent agent;

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        playerActionAsset = new NightDetective2();
        move = playerActionAsset.Player.Move;
        fire = playerActionAsset.Player.Fire;
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        playerActionAsset.Enable();
        fire.performed += CheckNavMesh;
    }

    private void OnDisable()
    {
        playerActionAsset.Disable();
        fire.performed -= CheckNavMesh;
    }

    private void Update()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("VirtualCamera is not assigned!");
            return;
        }

        Vector2 input = move.ReadValue<Vector2>();

        if (input != Vector2.zero)
        {
            agent.isStopped = true;
            agent.destination = transform.position;
        }
        else
        {
            agent.isStopped = false;
        }

        Vector3 forward = virtualCamera.transform.forward;
        Vector3 right = virtualCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * input.y + right * input.x;
        moveDirection.Normalize();

        transform.Translate(moveDirection * maxSpeed * Time.deltaTime, Space.World);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Selectable"))
        {
            Debug.Log("Entered Selectable trigger");
            cameraManager.SwitchCamera(cameraManager.zoomin);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Selectable"))
        {
            Debug.Log("Exited Selectable trigger");
            cameraManager.SwitchCamera(cameraManager.zoomout);
        }
    }

    private void CheckNavMesh(InputAction.CallbackContext context)
    {
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
        {
            agent.destination = hit.point;
            Debug.Log("Hit point is within the NavMesh.");
        }
        else
        {
            Debug.Log("Hit point is outside the NavMesh.");
        }
    }
}
