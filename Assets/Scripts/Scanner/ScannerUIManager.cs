using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScannerUIManager : MonoBehaviour
{
    [Header("Scan Progress UI")]
    public GameObject ScanUI;
    public Slider scanProgressSlider;
    public TextMeshProUGUI scanProgressText;
    [SerializeField] private int totalScannables = 15;

    private HashSet<ScanData> uniqueScans = new HashSet<ScanData>();

    public void AddScan(ScanData data)
    {
        if (data == null) return;

        if (uniqueScans.Add(data)) 
        {
            UpdateProgress();
        }
    }

    private void UpdateProgress()
    {
        if (scanProgressSlider != null)
        {
            scanProgressSlider.maxValue = totalScannables;
            scanProgressSlider.value = uniqueScans.Count;
        }

        if (scanProgressText != null)
        {
            float percent = (float)uniqueScans.Count / totalScannables * 100f;
            scanProgressText.text = $"{uniqueScans.Count} / {totalScannables} scanned ({percent:F0}%)";
        }
    }


    public void UpdateScanProgress(float normalizedProgress)
    {
        if (scanProgressSlider != null)
        {
            scanProgressSlider.value = Mathf.Clamp01(normalizedProgress) * totalScannables;
        }
    }


}
