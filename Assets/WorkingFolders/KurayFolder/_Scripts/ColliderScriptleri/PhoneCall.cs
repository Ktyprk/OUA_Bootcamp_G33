using System.Collections; // IEnumerator için gerekli
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhoneCall : MonoBehaviour
{
    public string sceneToLoad; // Inspector'dan sahneyi belirlemek için

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource bileþeni bulunamadý!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            StartCoroutine(WaitForAudioToEnd());
        }
    }

    private IEnumerator WaitForAudioToEnd()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        SceneManager.LoadScene(sceneToLoad);
    }
}
