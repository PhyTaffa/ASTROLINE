using System;
using UnityEngine;

public class PlayerButtonDetector : MonoBehaviour{
 
    public GameObject buttonArea;
    public ShipCameraManager cameraManager;
    public GameObject interactPrompt;

    private void Awake(){
        
        interactPrompt.SetActive(false);
    }

    void OnTriggerEnter(Collider other){
  
        if (other.gameObject == buttonArea){
            cameraManager.SetCanSwitch(true);
            interactPrompt.SetActive(true);    
        }
    }

    void OnTriggerExit(Collider other){
        
        if (other.gameObject == buttonArea) {
            
            cameraManager.SetCanSwitch(false);
            interactPrompt.SetActive(false);
        }
    }
}