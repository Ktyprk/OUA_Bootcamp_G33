using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomLightColor : MonoBehaviour
{
    public List<Light> pointLights;
    public float colorChangeInterval = 1f;

    private void Start()
    {
        if (pointLights == null || pointLights.Count == 0)
        {
            Debug.LogWarning("Point lights list is empty or not assigned.");
            return;
        }

        foreach (Light light in pointLights)
        {
            if (light.type == LightType.Point)
            {
                StartCoroutine(ChangeLightColor(light));
            }
        }
    }

    private IEnumerator ChangeLightColor(Light light)
    {
        while (true)
        {
            light.color = new Color(Random.value, Random.value, Random.value);
            yield return new WaitForSeconds(colorChangeInterval);
        }
    }
}
