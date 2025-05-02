using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScannerUIManager : MonoBehaviour {

    private const string prefsKey = "HasScannedAny";
    
    public GameObject ScanUI;
    public Slider scanProgressSlider;
    public TextMeshProUGUI scanProgressText;
    [SerializeField] private int totalScannables = 15;

    private HashSet<ScanData> uniqueScans = new HashSet<ScanData>();

    
    public static bool HasScanned { get; private set; }

    void Awake() {
        
        HasScanned = PlayerPrefs.GetInt(prefsKey, 0) == 1;
        
    }

    
    public void AddScan(ScanData data) {


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
