using System.Collections;
using UnityEngine;

public class CarLights : MonoBehaviour
{
    [SerializeField] private Light redLight;
    [SerializeField] private Light blueLight;
    [SerializeField] private float flashDuration = 0.1f; // I��klar�n yan�p s�nme s�resi
    [SerializeField] private int flashesPerCycle = 3; // Her �����n ka� kez yan�p s�nece�i

    private void Start()
    {
        StartCoroutine(PoliceLightsCoroutine());
    }

    private IEnumerator PoliceLightsCoroutine()
    {
        while (true)
        {
            // K�rm�z� ���k yan�p s�n�yor
            for (int i = 0; i < flashesPerCycle; i++)
            {
                redLight.enabled = true;
                yield return new WaitForSeconds(flashDuration);
                redLight.enabled = false;
                yield return new WaitForSeconds(flashDuration);
            }

            // Mavi ���k yan�p s�n�yor
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
