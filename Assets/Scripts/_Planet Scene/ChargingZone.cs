using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingZone : MonoBehaviour {
    
    [SerializeField] private BatteryUI batteryUI;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered charging zone");
            batteryUI.SetChargingZone(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exited charging zone");
            batteryUI.SetChargingZone(false);
        }
    }

}
