using System.Collections;
using UnityEngine;

public class CarLights : MonoBehaviour
{
    [SerializeField] private Light redLight;
    [SerializeField] private Light blueLight;
    [SerializeField] private float flashDuration = 0.1f; // Iþýklarýn yanýp sönme süresi
    [SerializeField] private int flashesPerCycle = 3; // Her ýþýðýn kaç kez yanýp söneceði

    private void Start()
    {
        StartCoroutine(PoliceLightsCoroutine());
    }

    private IEnumerator PoliceLightsCoroutine()
    {
        while (true)
        {
            // Kýrmýzý ýþýk yanýp sönüyor
            for (int i = 0; i < flashesPerCycle; i++)
            {
                redLight.enabled = true;
                yield return new WaitForSeconds(flashDuration);
                redLight.enabled = false;
                yield return new WaitForSeconds(flashDuration);
            }

            // Mavi ýþýk yanýp sönüyor
            for (int i = 0; i < flashesPerCycle; i++)
            {
                blueLight.enabled = true;
                yield return new WaitForSeconds(flashDuration);
                blueLight.enabled = false;
                yield return new WaitForSeconds(flashDuration);
            }
        }
    }
}
