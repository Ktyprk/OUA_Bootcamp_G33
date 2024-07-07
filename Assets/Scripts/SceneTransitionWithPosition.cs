using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionWithPosition : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName;
    
    [SerializeField]
    private string targetPositionName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetString("TargetScene", targetSceneName);
            PlayerPrefs.SetString("TargetPosition", targetPositionName);
            
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
