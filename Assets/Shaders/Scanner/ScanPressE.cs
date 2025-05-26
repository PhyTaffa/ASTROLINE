using System.Collections.Generic;
using UnityEngine;
public class ScanPressE : MonoBehaviour
{
    [Tooltip("Which material slot to use for the outline on each MeshRenderer")]
    [SerializeField] private int outlineMaterialSlot = 1;
    [Tooltip("Speed of the rainbow cycle (hue change per second)")]
    [SerializeField] private float rainbowSpeed = 1f;

    // All the renderers under this parent
    private MeshRenderer[] _renderers;

    // For each renderer we store:
    //  - its original Material[] array
    //  - our instantiated outline Material instance
    private List<Material[]>     _originalMaterialArrays = new List<Material[]>();
    private List<Material>       _outlineInstances       = new List<Material>();

    // State
    private bool   _outlineOn = false;
    private Color  _defaultColor;
    private enum  ColorMode { Default, Red, Rainbow }
    private ColorMode _mode = ColorMode.Default;

    void Awake()
    {
        // grab every MeshRenderer in children (including this one)
        _renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (var rend in _renderers)
        {
            // 1) store a deep copy of its original materials
            var originals = rend.materials;
            _originalMaterialArrays.Add((Material[])originals.Clone());

            // 2) instantiate slot N so we have a unique copy we can recolor
            Material outlineMat = null;
            if (originals.Length > outlineMaterialSlot)
            {
                outlineMat = Instantiate(originals[outlineMaterialSlot]);
                var mats = (Material[])originals.Clone();
                mats[outlineMaterialSlot] = outlineMat;
                rend.materials = mats;
            }
            _outlineInstances.Add(outlineMat);
        }

        // remember whatever color your shader had in that slot originally
        // (take from the first renderer)
        if (_outlineInstances[0] != null)
            _defaultColor = _outlineInstances[0].GetColor("_OutlineColor");
        else
            _defaultColor = Color.green;

        Debug.Log($"ScanPressE: Found {_renderers.Length} renderers. Default color={_defaultColor}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) ToggleOutline();
        if (Input.GetKeyDown(KeyCode.R)) CycleRedDefault();
        if (Input.GetKeyDown(KeyCode.T)) ToggleRainbow();

        // if rainbow mode is active and outline is on, animate hue
        if (_outlineOn && _mode == ColorMode.Rainbow)
            UpdateRainbow();
    }

    private void ToggleOutline()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            var rend = _renderers[i];
            var mats = (Material[])_originalMaterialArrays[i].Clone();

            if (_outlineOn)
            {
                // restore original
                mats[outlineMaterialSlot] = _originalMaterialArrays[i][outlineMaterialSlot];
            }
            else
            {
                // swap in our instance
                mats[outlineMaterialSlot] = _outlineInstances[i];
            }

            rend.materials = mats;
        }

        _outlineOn = !_outlineOn;
        Debug.Log($"ScanPressE: Outline {(_outlineOn ? "ON" : "OFF")}");
    }

    private void CycleRedDefault()
    {
        if (_mode != ColorMode.Red)
        {
            _mode = ColorMode.Red;
            SetAllOutlineColors(Color.red);
            Debug.Log("ScanPressE: Mode → Red");
        }
        else
        {
            _mode = ColorMode.Default;
            SetAllOutlineColors(_defaultColor);
            Debug.Log("ScanPressE: Mode → Default");
        }
    }

    private void ToggleRainbow()
    {
        if (_mode != ColorMode.Rainbow)
        {
            _mode = ColorMode.Rainbow;
            Debug.Log("ScanPressE: Mode → Rainbow");
        }
        else
        {
            _mode = ColorMode.Default;
            SetAllOutlineColors(_defaultColor);
            Debug.Log("ScanPressE: Mode → Default");
        }
    }

    private void UpdateRainbow()
    {
        float hue = (Time.time * rainbowSpeed) % 1f;
        var c = Color.HSVToRGB(hue, 1f, 1f);
        SetAllOutlineColors(c);
    }

    private void SetAllOutlineColors(Color c)
    {
        foreach (var mat in _outlineInstances)
        {
            if (mat != null)
                mat.SetColor("_OutlineColor", c);
        }
    }
}
