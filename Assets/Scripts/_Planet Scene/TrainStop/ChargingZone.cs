using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingZone : MonoBehaviour {
    
    [SerializeField] private BatteryUI batteryUI;
    [SerializeField] private ChargingState manager;   
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered charging zone");
            batteryUI.SetChargingZone(true);
            manager.isCharging = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exited charging zone");
            batteryUI.SetChargingZone(false);
            manager.isCharging = false;
        }
    }

}
