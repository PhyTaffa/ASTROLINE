using UnityEngine;

public class ChargingState : MonoBehaviour {
    [SerializeField] private GameObject chargingOn;
    [HideInInspector] public bool isCharging;

    void Awake(){
      
        isCharging = false;
        chargingOn.SetActive(false);
    }

    void Update(){
        chargingOn.SetActive(isCharging);
    }
}