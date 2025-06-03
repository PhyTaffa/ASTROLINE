using System;
using UnityEngine;

public class PlayerButtonDetector : MonoBehaviour
{
    public GameObject shipComputerButtonArea;
    public GameObject cargoComputerButtonArea; 
    public GameObject playButtonArea;
    public GameObject door1ButtonArea;
    public GameObject door2ButtonArea;
    public ShipCameraManager cameraManager;
    public GameObject shipInteractPrompt;
    public GameObject cargoInteractPrompt;
    public GameObject door1InteractPrompt;
    public GameObject door2InteractPrompt;
    public GameObject door1GoPrompt;
    public GameObject door2GoPrompt;
    
    private void Awake(){
        shipInteractPrompt.SetActive(false);
        cargoInteractPrompt.SetActive(false);
        playButtonArea.SetActive(false);
        door1InteractPrompt.SetActive(false);
        door2InteractPrompt.SetActive(false);
        door1GoPrompt.SetActive(false);
        door2GoPrompt.SetActive(false);
    }

    void OnTriggerEnter(Collider other){
  
        if (other.gameObject == shipComputerButtonArea){
            cameraManager.SetShipSwitch(true);
            shipInteractPrompt.SetActive(true);    
        }
        
        if (other.gameObject == cargoComputerButtonArea){
            cameraManager.SetCargoSwitch(true);
            cargoInteractPrompt.SetActive(true);    
            playButtonArea.SetActive(true);
        }
        
        if (other.gameObject == door1ButtonArea){
            door1InteractPrompt.SetActive(true);    
            door1GoPrompt.SetActive(true);    
        }
        
        if (other.gameObject == door2ButtonArea){
            door2InteractPrompt.SetActive(true);    
            door2GoPrompt.SetActive(true);     
        }
    }

    void OnTriggerExit(Collider other){
        
        if (other.gameObject == shipComputerButtonArea){
            cameraManager.SetShipSwitch(false);
            shipInteractPrompt.SetActive(false);
        }
        
        if (other.gameObject == cargoComputerButtonArea){
            cameraManager.SetCargoSwitch(false);
            cargoInteractPrompt.SetActive(false);
            playButtonArea.SetActive(false);
        }
        
        if (other.gameObject == door1ButtonArea){
            door1InteractPrompt.SetActive(false);
            door1GoPrompt.SetActive(false);    
        }
        
        if (other.gameObject == door2ButtonArea){
            door2InteractPrompt.SetActive(false);
            door2GoPrompt.SetActive(false); 
        }
    }
}