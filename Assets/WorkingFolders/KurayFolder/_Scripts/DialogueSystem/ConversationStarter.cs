using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is within the trigger zone.");

            if (Input.GetKeyDown(KeyCode.F))
            {

                Debug.Log("F key pressed. Starting conversation.");
                ConversationManager.Instance.StartConversation(myConversation);
            }
        }
    }
}
