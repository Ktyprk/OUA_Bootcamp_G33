using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range = 10f; // The radius within which the agent will choose a random destination.
    public float minDistance = 2f; // Minimum distance to move to a new point.
    public float speed = 3.5f; // Speed of the agent.
    private Animator animator; // Animator component.
    private Vector3 lastPosition;
    private float stuckTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get the Animator component.
        agent.speed = speed; // Set the agent's speed.
        MoveToRandomPoint();
    }

    void Update()
    {
        // Check if the agent has reached its destination.
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // If the agent has stopped moving, set "IsWalking" to false.
            animator.SetBool("IsWalking", false);

            // Move to a new random point after a short delay.
            StartCoroutine(MoveAfterDelay(2f)); // 2-second delay before moving to the next point.
        }
        else
        {
            // If the agent is moving, set "IsWalking" to true.
            if (agent.velocity.magnitude > 0.1f) // Check if the agent is actually moving.
            {
                animator.SetBool("IsWalking", true);

                // Reset the stuck timer if the agent is moving.
                stuckTimer = 0f;
            }
            else
            {
                animator.SetBool("IsWalking", false);

                // Increment the stuck timer if the agent is not moving.
                stuckTimer += Time.deltaTime;

                // If the agent is stuck for more than 2 seconds, move to a new point.
                if (stuckTimer > 2f)
                {
                    MoveToRandomPoint();
                    stuckTimer = 0f;
                }
            }
        }

        // Check if the agent is stuck by comparing its position over time.
        if (Vector3.Distance(transform.position, lastPosition) < 0.1f)
        {
            stuckTimer += Time.deltaTime;

            // If the agent is stuck for more than 2 seconds, move to a new point.
            if (stuckTimer > 2f)
            {
                MoveToRandomPoint();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPosition = transform.position;
    }

    void MoveToRandomPoint()
    {
        Vector3 point;
        if (RandomPoint(transform.position, range, out point))
        {
            if (Vector3.Distance(transform.position, point) > minDistance)
            {
                agent.SetDestination(point);
            }
            else
            {
                // Try to find another point if the distance is too small
                MoveToRandomPoint();
            }
        }
        else
        {
            // If no valid point is found, try again
            MoveToRandomPoint();
        }
    }

    IEnumerator MoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        MoveToRandomPoint();
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range; // Generate a random point within a sphere.
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) // Check if the point is on the NavMesh.
            {
                if (Vector3.Distance(center, hit.position) >= minDistance) // Ensure the point is not too close
                {
                    result = hit.position;
                    return true;
                }
            }
        }
        result = Vector3.zero;
        return false;
    }
}
