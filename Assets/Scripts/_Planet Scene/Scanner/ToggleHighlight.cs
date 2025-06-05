using System.Collections.Generic;
using UnityEngine;

public class ToggleHighlight : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;

    private List<MeshRenderer> meshRenderers = new();
    private List<SkinnedMeshRenderer> skinnedMeshRenderers = new();
    private Material baseMaterial;
    private Material overlayInstance;
    private bool highlightOn = false;

    void Awake()
    {
        meshRenderers.AddRange(GetComponentsInChildren<MeshRenderer>());
        skinnedMeshRenderers.AddRange(GetComponentsInChildren<SkinnedMeshRenderer>());

        if (meshRenderers.Count > 0)
        {
            baseMaterial = meshRenderers[0].materials[0];
        }
        else if (skinnedMeshRenderers.Count > 0)
        {
            baseMaterial = skinnedMeshRenderers[0].materials[0];
        }
        else
        {
            Debug.LogError($"ToggleHighlight on '{name}' couldn't find any MeshRenderer or SkinnedMeshRenderer!");
            enabled = false;
            return;
        }
    }

    /// <summary>
    /// Toggle the overlay on/off. When on, creates a fresh instance
    /// of the highlight material and applies it.
    /// </summary>
    public void ToggleOutline()
    {
        highlightOn = !highlightOn;

        if (highlightOn)
        {
            overlayInstance = new Material(highlightMaterial);
            var mats = new List<Material> { baseMaterial, overlayInstance };

            foreach (var mr in meshRenderers)
                mr.materials = mats.ToArray();

            foreach (var smr in skinnedMeshRenderers)
                smr.materials = mats.ToArray();
        }
        else
        {
            var baseMats = new Material[] { baseMaterial };

            foreach (var mr in meshRenderers)
                mr.materials = baseMats;

            foreach (var smr in skinnedMeshRenderers)
                smr.materials = baseMats;

            overlayInstance = null;
        }
    }

    /// <summary>
    /// Set the _Mode property on the overlay material.
    /// Automatically enables highlighting if off.
    /// </summary>
    public void SetMode(int mode)
    {
        if (!highlightOn)
            ToggleOutline();

        if (overlayInstance != null)
            overlayInstance.SetFloat("_Mode", mode);
    }
}
