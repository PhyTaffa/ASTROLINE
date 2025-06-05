using System.Collections.Generic;
using UnityEngine;

public class ToggleHighlight : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;

    private MeshRenderer meshRenderer;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Material baseMaterial;
    private Material overlayInstance;
    private bool highlightOn = false;

    void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        if (meshRenderer != null)
        {
            baseMaterial = meshRenderer.materials[0];
        }
        else if (skinnedMeshRenderer != null)
        {
            baseMaterial = skinnedMeshRenderer.materials[0];
        }
        else
        {
            Debug.LogError($"ToggleHighlight on '{name}' couldn't find a MeshRenderer or SkinnedMeshRenderer!");
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

            if (meshRenderer != null)
                meshRenderer.materials = mats.ToArray();
            else if (skinnedMeshRenderer != null)
                skinnedMeshRenderer.materials = mats.ToArray();
        }
        else
        {
            if (meshRenderer != null)
                meshRenderer.materials = new Material[] { baseMaterial };
            else if (skinnedMeshRenderer != null)
                skinnedMeshRenderer.materials = new Material[] { baseMaterial };

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
