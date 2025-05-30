using UnityEngine;
using System.Collections.Generic;

public class ToggleHighlight : MonoBehaviour {
    [SerializeField] private Material highlightMaterial;

    private MeshRenderer meshRenderer;
    private Material baseMaterial;
    private bool highlightOn = false;

    void Awake(){
        meshRenderer  = GetComponent<MeshRenderer>();
        baseMaterial  = meshRenderer.materials[0];
    }

    // (Optional) you can also remove this Update entirely,
    // since your Scanner now drives the toggle:
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //         ToggleOutline();
    // }

    // ← make this public so Scanner can see it
    public void ToggleOutline()
    {
        var mats = new List<Material> { baseMaterial };

        if (!highlightOn)
            mats.Add(highlightMaterial);

        meshRenderer.materials = mats.ToArray();
        highlightOn = !highlightOn;
    }
}
