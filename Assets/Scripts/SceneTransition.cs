using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Hangi sahneye geçiþ yapýlacaðýný belirten deðiþken
    [SerializeField]
    private string targetSceneName;

    // Collider'a bir þey girdiðinde çaðrýlýr
    void OnTriggerEnter(Collider other)
    {
        // Eðer Collider'a giren nesne oyuncu ise (Tag ile kontrol ediliyor)
        if (other.CompareTag("Player"))
        {
            // Hedef sahneye geçiþ yap
            SceneManager.LoadScene("karakol2.kat");
        }
    }
}

