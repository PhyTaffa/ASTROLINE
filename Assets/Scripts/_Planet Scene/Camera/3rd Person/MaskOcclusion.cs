using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskOcclusion : MonoBehaviour
{
    public Transform player;
    public LayerMask occlusionMask;
    private List<Renderer> previousRenderers = new List<Renderer>();

    void LateUpdate()
    {
        // Clear previously faded objects
        foreach (Renderer rend in previousRenderers)
        {
            SetMaterialFade(rend, false);
        }
        previousRenderers.Clear();

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance, occlusionMask);

        foreach (RaycastHit hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null)
            {
                SetMaterialFade(rend, true);
                previousRenderers.Add(rend);
            }
        }
    }
    
    void SetMaterialFade(Renderer rend, bool makeTransparent)
    {
        foreach (Material mat in rend.materials)
        {
            if (makeTransparent)
            {
                mat.SetFloat("_Mode", 2); // Fade mode
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;

                Color c = mat.color;
                c.a = 0.3f;
                mat.color = c;
            }
            else
            {
                Color c = mat.color;
                c.a = 1f;
                mat.color = c;
            }
        }
    }
    
}
