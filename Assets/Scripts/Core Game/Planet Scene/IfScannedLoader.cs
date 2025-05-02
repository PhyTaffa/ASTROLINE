using UnityEngine;

public class IfScannedLoader : MonoBehaviour {
    
    // Temporary Indicator to help me :)
    public GameObject scannedIndicator;
    // Use the matching key name from "Scanner" script HasScannedSlug or HasScannedFrog
    public string prefsKey;

    void Awake() {
        
        if (scannedIndicator != null) {
            scannedIndicator.SetActive(false);
        }
    }

    void Update() {
        
        if (scannedIndicator == null) return;

        bool hasScanned = PlayerPrefs.GetInt(prefsKey, 0) == 1;
        
        if (scannedIndicator.activeSelf != hasScanned){
            scannedIndicator.SetActive(hasScanned);
        }
           
    }
}