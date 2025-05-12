using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannableObject : MonoBehaviour 
{
    public ScanData scanData;

    private void Awake() {
        // If no scan data is assigned, load the generic one
        if (scanData == null) {
            
            //it's kworking, dont worry about messages about "happy" shit
            scanData = Resources.Load<ScanData>("Scriptable Object/ScanData/GenericScanData");
            
            if (scanData == null){
                
                Debug.LogError("GenericScanData not found in Resources!");
                
            }else {
                Debug.Log($"GenericScanData assigned to: {gameObject.name}");
            }
        }
    }
}

