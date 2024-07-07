using UnityEngine;

public class CeilingTransparency : MonoBehaviour
{
    public GameObject ceiling; // Þeffaflaþtýrýlacak tavan objesi
    public Material transparentMaterial; // Þeffaf materyal
    private Material originalMaterial; // Orijinal materyal
    private Renderer ceilingRenderer;

    void Start()
    {
        ceilingRenderer = ceiling.GetComponent<Renderer>();
        originalMaterial = ceilingRenderer.material;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ceilingRenderer.material = transparentMaterial;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ceilingRenderer.material = originalMaterial;
        }
    }
}
