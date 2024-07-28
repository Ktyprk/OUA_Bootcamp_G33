using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationStartedConversationFinished : MonoBehaviour
{
    public string playerTag = "Player";
    public Animator animator;
    public float rotationSpeed = 2.0f;
    private Transform player;
    private Quaternion initialRotation;
    private bool isPlayerNearby = false;
    private bool isRotating = false;

    void Start()
    {
        // Baþlangýç rotasyonunu sakla
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("StartConversation");
            isRotating = true;
        }

        if (isRotating && player != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // NPC'nin yüzü tamamen oyuncuya döndüðünde rotasyonu durdur
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1.0f)
            {
                isRotating = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            player = other.transform;
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            animator.SetTrigger("FinishConversation");
            isPlayerNearby = false;
            player = null;

            // Baþlangýç rotasyonuna geri dön
            StartCoroutine(RotateToInitialRotation());
        }
    }

    private IEnumerator RotateToInitialRotation()
    {
        while (Quaternion.Angle(transform.rotation, initialRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        transform.rotation = initialRotation;
    }
}
