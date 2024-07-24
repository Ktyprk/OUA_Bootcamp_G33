using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Transform[] waypoints;
    public float detectionRadius = 10f;
    public float stopDistance = 2f;
    
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRadius)
            {
                LookAtPlayer();
                
                if (distanceToPlayer <= stopDistance)
                {
                    // Oyuncuya bak
                    LookAtPlayer();
                    // Hareketi durdur
                    agent.isStopped = true;
                }
                else
                {
                    // Oyuncuyu takip et
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                }
            }
            else
            {
                // Waypoint'leri takip et
                agent.isStopped = false;
                if (agent.remainingDistance < 0.5f && !agent.pathPending)
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                    agent.SetDestination(waypoints[currentWaypointIndex].position);
                }
            }

    }
    
    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 500f);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = Color.green;
        foreach (Transform waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint.position, 0.5f);
        }
    }

    private void OnDisable()
    {
        
    }
}
