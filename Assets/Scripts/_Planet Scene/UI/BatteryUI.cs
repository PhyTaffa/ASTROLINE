using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryUI : MonoBehaviour {

    [SerializeField] private GameObject[] batteryStates;

    private float batteryTimer = 10f;
    private float maxTime = 10f;
    private int currentBatteryIndex = 0;
    private bool isChargingZone = false; 

    // Log logic
    private int lastLoggedStep = -1;
    
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
            batteryTimer -= Time.deltaTime * 0.1f;
        }

        batteryTimer = Mathf.Clamp(batteryTimer, 0f, maxTime);
        
        // Loging Percentage wowzers (10& at a time)we
        float percent = (batteryTimer / maxTime) * 100f;
        int step = Mathf.FloorToInt(percent / 10f) * 10;
        if (step != lastLoggedStep) {
            lastLoggedStep = step;
            Debug.Log($"Battery: {step}%");
        }
        
        UpdateBatteryUI();
    }

    void UpdateBatteryUI() {
        int batteryLevel = 0;

        if (batteryTimer > 8f) batteryLevel = 0;
        else if (batteryTimer > 6f) batteryLevel = 1;
        else if (batteryTimer > 4f) batteryLevel = 2;
        else if (batteryTimer > 0.01f) batteryLevel = 3;
        else if (batteryTimer <= 0.01f) batteryLevel = 4;

        currentBatteryIndex = batteryLevel; // <-- Save current battery state index
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
    
    public int GetBatteryLevelIndex() {
        return currentBatteryIndex;
    }
}