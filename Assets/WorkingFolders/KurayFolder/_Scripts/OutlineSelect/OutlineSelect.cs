using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineSelect : MonoBehaviour
{
    [Range(0.1f, 100f)]
    public float gizmoRadius = 5f; // Gizmo'nun yarýçapý
    private Transform selection;

    void Update()
    {
        // Selection
        if (selection != null)
        {
            selection.gameObject.GetComponent<Outline>().enabled = false;
            selection = null;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, gizmoRadius);
        foreach (var hitCollider in hitColliders)
        {
            Transform hitTransform = hitCollider.transform;
            if (hitTransform.CompareTag("Selectable"))
            {
                if (hitTransform.gameObject.GetComponent<Outline>() != null)
                {
                    hitTransform.gameObject.GetComponent<Outline>().enabled = true;
                    Debug.Log($"Outline enabled for {hitTransform.name}");
                }
                else
                {
                    Outline outline = hitTransform.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    outline.OutlineColor = Color.magenta;
                    outline.OutlineWidth = 7.0f;
                    Debug.Log($"Outline component added and enabled for {hitTransform.name}");
                }
                selection = hitTransform;
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}
