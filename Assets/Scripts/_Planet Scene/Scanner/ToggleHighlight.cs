        using UnityEngine;
using System.Collections.Generic;

public class ToggleHighlight : MonoBehaviour {
    [SerializeField] private Material highlightMaterial;

    private MeshRenderer meshRenderer;
    private Material baseMaterial;
    private Material overlayInstance;
    private bool highlightOn = false;

    void Awake(){
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        if(meshRenderer == null){
            Debug.LogError($"ToggleHighlight on '{name}' couldn't find a MeshRenderer!");
            enabled = false;
            
            return;
        }
        baseMaterial  = meshRenderer.materials[0];
    }

    /// <summary>
    /// Toggle the overlay on/off.  When on, we create a fresh instance
    /// of the shader‐graph material so we can drive its _Mode.
    /// </summary>
    public void ToggleOutline()
    {
        highlightOn = !highlightOn;

        if (highlightOn)
        {
            // make one instance copy so setting _Mode won't affect
            // every object using that same highlightMaterial asset
            overlayInstance = new Material(highlightMaterial);

            // build array: [ baseMaterial, overlayInstance ]
            var mats = new List<Material> { baseMaterial, overlayInstance };
            meshRenderer.materials = mats.ToArray();
        }
        else
        {
            // just revert to only the base
            meshRenderer.materials = new Material[] { baseMaterial };
            overlayInstance = null;
        }
    }

    /// <summary>
    /// Drive the shader-graph's Mode property: 0=Red, 1=Green, 2=Rainbow.
    /// If overlay isn't currently on, we'll turn it on for you.
    /// </summary>
    public void SetMode(int mode)
    {
        if (!highlightOn)
            ToggleOutline();

        // safety check
        if (overlayInstance != null)
            overlayInstance.SetFloat("_Mode", mode);
    }
}