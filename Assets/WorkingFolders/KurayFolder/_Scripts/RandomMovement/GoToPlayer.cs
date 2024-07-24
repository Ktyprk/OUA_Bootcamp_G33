using UnityEngine;
using UnityEngine.AI;
using DialogueEditor;
using System.Collections;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private float originalSpeed = 3.5f; // H�z� saklamak i�in

    private NavMeshAgent agent;
    private Transform playerTransform;
    private Rigidbody rb;
    private bool isPlayerInside = false;
    private bool isMovingToRandomPosition = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>(); // Rigidbody bile�enini al

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player tag'ine sahip obje bulunamad�!");
        }

        // Orijinal h�z de�erini sakla
        originalSpeed = agent.speed;
    }

    void Update()
    {
        if (playerTransform != null && !isPlayerInside && !isMovingToRandomPosition)
        {
            // Player'a do�ru hareket et
            agent.SetDestination(playerTransform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            agent.isStopped = true; // Hareketi durdur
            StopCoroutine(MoveToRandomPosition());

            // H�z� s�f�r yap
            agent.speed = 0f;

            // Konu�may� ba�lat
            if (ConversationManager.Instance != null)
            {
                ConversationManager.Instance.StartConversation(myConversation);
            }

            // Player'a bak
            transform.LookAt(playerTransform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            isMovingToRandomPosition = true; // Rastgele pozisyonlara gitme modu aktif

            // H�z� eski de�erine d�nd�r
            agent.speed = originalSpeed;


            StartCoroutine(MoveToRandomPosition());
        }
    }

    private IEnumerator MoveToRandomPosition()
    {
        while (isMovingToRandomPosition)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
            Vector3 finalPosition = hit.position;

            // Hedefi rastgele pozisyona ayarla ve hareket et
            agent.SetDestination(finalPosition);
            agent.isStopped = false; // Hareketi ba�lat

            yield return new WaitForSeconds(5f); // Rastgele pozisyona gitme s�resi
        }
    }
}
