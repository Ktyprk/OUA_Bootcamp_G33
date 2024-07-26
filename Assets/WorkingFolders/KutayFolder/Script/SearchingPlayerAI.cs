using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DialogueEditor;

public class SearchingPlayerAI : MonoBehaviour
{
   
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private float originalSpeed = 3.5f; 
    public float stopDistance = 2f;
    private NavMeshAgent agent;
    private Transform playerTransform;
    private bool isMovingToRandomPosition;
    
    public Animator _animator;


    private GameObject player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        originalSpeed = agent.speed;
    }

    void Update()
    {
        if (playerTransform != null && !isMovingToRandomPosition)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            
            if (distanceToPlayer <= stopDistance)
            {
                LookAtPlayer();
                agent.isStopped = true;
                this._animator.SetBool("isWalking", false);
            }
            else
            {
                agent.isStopped = false;
                this._animator.SetBool("isWalking", true);
                agent.SetDestination(playerTransform.position);
            }
        }
        else if (isMovingToRandomPosition)
        {
            MoveToRandomPosition();
        }
        float player = Vector3.Distance(transform.position, playerTransform.position);

        // if (player <= stopDistance)
        // {
        //     this._animator.SetBool("isWalking", false);
        // }
        // else
        // {
        //     agent.SetDestination(playerTransform.position);
        // }
    }

    private void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
        Vector3 finalPosition = hit.position;
        
        agent.SetDestination(finalPosition);
        agent.isStopped = false;
    }
    
    private void LookAtPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 500f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isMovingToRandomPosition = false;
            agent.isStopped = true;

            if (ConversationManager.Instance != null)
            {
                agent.speed = 0;
                ConversationManager.Instance.StartConversation(myConversation);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isMovingToRandomPosition = true;
            agent.speed = originalSpeed;
        }
    }

}
