using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryUI : MonoBehaviour {

    [SerializeField] private GameObject[] batteryStates;

    private float batteryTimer = 10f;
    private float maxTime = 10f;

    private bool isChargingZone = false; 
    // Set this via OnTriggerEnter/Exit later

    void Start() {
        SetBatteryLevel(0);
    }

    void Update() {
        
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            batteryTimer = maxTime;
        }

        if (isChargingZone) {
            
            batteryTimer += Time.deltaTime;
            
        } else {
            batteryTimer -= Time.deltaTime;
        }

        batteryTimer = Mathf.Clamp(batteryTimer, 0f, maxTime);
        UpdateBatteryUI();
    }

    void UpdateBatteryUI() {
        int batteryLevel = 0;

        if (batteryTimer > 8f) batteryLevel = 0;
        else if (batteryTimer > 6f) batteryLevel = 1;
        else if (batteryTimer > 4f) batteryLevel = 2;
        else if (batteryTimer > 2f) batteryLevel = 3;
        else batteryLevel = 4;

        SetBatteryLevel(batteryLevel);
    }

    public void SetBatteryLevel(int index) {
        for (int i = 0; i < batteryStates.Length; i++) {
            batteryStates[i].SetActive(i == index);
        }
    }

    //add this trigger zone logic later:
    public void SetChargingZone(bool inZone) {
        isChargingZone = inZone;
    }
}