using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionWithPosition : MonoBehaviour
{
    // Geçiþ yapýlacak sahne adý
    [SerializeField]
    private string targetSceneName;

    // Hedef konum adý (1. katýn sað mý sol mu? 2. katýn sað mý sol mu?)
    [SerializeField]
    private string targetPositionName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hedef sahne adý ve pozisyon adý PlayerPrefs'e kaydediliyor
            PlayerPrefs.SetString("TargetScene", targetSceneName);
            PlayerPrefs.SetString("TargetPosition", targetPositionName);

            // Sahne deðiþtiriliyor
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
