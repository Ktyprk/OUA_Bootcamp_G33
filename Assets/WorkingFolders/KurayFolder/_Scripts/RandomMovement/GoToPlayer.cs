using UnityEngine;
using UnityEngine.AI;
using DialogueEditor;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private float originalSpeed = 3.5f; // Hýzý saklamak için
    public float stopDistance = 2f;
    private NavMeshAgent agent;
    private Transform playerTransform;
    private bool isMovingToRandomPosition;

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

            Debug.Log(distanceToPlayer);

            if (distanceToPlayer <= stopDistance)
            {
                LookAtPlayer();
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(playerTransform.position);
            }
        }
        else if (isMovingToRandomPosition)
        {
            MoveToRandomPosition();
        }
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