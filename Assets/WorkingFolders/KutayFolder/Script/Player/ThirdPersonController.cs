using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.AI;

public class ThirdPersonController : MonoBehaviour
{
    private ThirdPersonActionAsset playerActionAsset;
    private InputAction move, look, fire;
    public NavMeshAgent agent;
    
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    private void Awake()
    {
        playerActionAsset = new ThirdPersonActionAsset();
        move = playerActionAsset.Player.Move;
        look = playerActionAsset.Player.Look;
        fire = playerActionAsset.Player.Fire;
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        playerActionAsset.Enable();
        look.performed += Look;
        look.canceled += Look;
        fire.performed += CheckNavMesh;
    }

    private void OnDisable()
    {
        playerActionAsset.Disable();
        look.performed -= Look;
        look.canceled -= Look;
        fire.performed -= CheckNavMesh;
    }

    private void Update()
    {
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

        LookAt(input);

        Vector3 forward = freeLookCamera.transform.forward;
        Vector3 right = freeLookCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
    
        Vector3 moveDirection = forward * input.y + right * input.x;
        moveDirection.Normalize();
        
        transform.Translate(moveDirection * maxSpeed * Time.deltaTime, Space.World);
    }

    private void LookAt(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            Vector3 forward = freeLookCamera.transform.forward;
            Vector3 right = freeLookCamera.transform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 direction = input.x * right + input.y * forward;

            // Calculate the rotation towards the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Look(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 300f;
        }
        else if (context.canceled)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 0f;
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
