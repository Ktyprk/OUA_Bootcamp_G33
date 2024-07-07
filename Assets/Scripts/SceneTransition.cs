using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Hangi sahneye ge�i� yap�laca��n� belirten de�i�ken
    [SerializeField]
    private string targetSceneName;

    // Collider'a bir �ey girdi�inde �a�r�l�r
    void OnTriggerEnter(Collider other)
    {
        // E�er Collider'a giren nesne oyuncu ise (Tag ile kontrol ediliyor)
        if (other.CompareTag("Player"))
        {
            // Hedef sahneye ge�i� yap
            SceneManager.LoadScene("karakol2.kat");
        }
    }
}

