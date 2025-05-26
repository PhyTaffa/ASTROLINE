using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CombineMesh : MonoBehaviour
{
    [Tooltip("Your normal/base material")]
    [SerializeField] private Material baseMaterial;
    [Tooltip("Your outline material (shader must do an expanded-vertex pass)")]
    [SerializeField] private Material outlineMaterial;

    void Awake()
    {
        Debug.Log($"[CombineMesh] Starting on '{name}'");

        // 1) Gather all child MeshFilters (skip our own empty one):
        var filters = GetComponentsInChildren<MeshFilter>();
        var combines = new List<CombineInstance>();

        foreach (var mf in filters)
        {
            if (mf.transform == transform) continue;          // skip the root’s
            if (mf.sharedMesh == null) 
            {
                Debug.LogWarning($"[CombineMesh] '{mf.name}' has no mesh, skipping.");
                continue;
            }

            // Bring the child's mesh into parent-local space:
            var ci = new CombineInstance
            {
                mesh      = mf.sharedMesh,
                transform = transform.worldToLocalMatrix * mf.transform.localToWorldMatrix
            };
            combines.Add(ci);

            // Disable the child's own renderer
            var mr = mf.GetComponent<MeshRenderer>();
            if (mr != null) mr.enabled = false;

            Debug.Log($"[CombineMesh] Queued '{mf.name}' (verts={ci.mesh.vertexCount})");
        }

        if (combines.Count == 0)
        {
            Debug.LogWarning("[CombineMesh] No meshes to combine—aborting.");
            return;
        }

        // 2) Create & assign the merged mesh on our parent:
        var rootFilter = GetComponent<MeshFilter>();
        var merged    = new Mesh { name = name + "_Combined" };
        merged.CombineMeshes(combines.ToArray(), true, true);
        rootFilter.sharedMesh = merged;
        Debug.Log($"[CombineMesh] Combined into '{merged.name}' (total verts={merged.vertexCount})");

        // 3) Setup the “base” renderer on this GameObject:
        var baseRenderer = GetComponent<MeshRenderer>();
        baseRenderer.sharedMaterial = baseMaterial;

        // 4) Create a child GameObject just for the outline pass:
        var outlineGO = new GameObject("Outline");
        outlineGO.transform.SetParent(transform, false);
        outlineGO.transform.localPosition = Vector3.zero;
        outlineGO.transform.localRotation = Quaternion.identity;

        var ofFilter = outlineGO.AddComponent<MeshFilter>();
        ofFilter.sharedMesh = merged;

        var ofRenderer = outlineGO.AddComponent<MeshRenderer>();
        ofRenderer.sharedMaterial = outlineMaterial;

        Debug.Log($"[CombineMesh] Outline child created with '{outlineMaterial.name}'");
    }
}
