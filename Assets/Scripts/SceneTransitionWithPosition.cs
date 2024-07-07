using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionWithPosition : MonoBehaviour
{
    // Ge�i� yap�lacak sahne ad�
    [SerializeField]
    private string targetSceneName;

    // Hedef konum ad� (1. kat�n sa� m� sol mu? 2. kat�n sa� m� sol mu?)
    [SerializeField]
    private string targetPositionName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hedef sahne ad� ve pozisyon ad� PlayerPrefs'e kaydediliyor
            PlayerPrefs.SetString("TargetScene", targetSceneName);
            PlayerPrefs.SetString("TargetPosition", targetPositionName);

            // Sahne de�i�tiriliyor
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
