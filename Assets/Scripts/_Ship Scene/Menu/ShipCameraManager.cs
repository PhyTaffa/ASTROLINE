using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCameraManager : MonoBehaviour {
    public Camera shipCamera;
    public Camera planetCamera;
    public GameObject interactPrompt; 
    public GameObject computerUI;
    public ShipPlayerController movementController; 
    public ComputerUISlider sliderUiLogic;
    
    private bool usingAlt = false;
    private bool canSwitch = false;

    private void Awake() {
        computerUI.SetActive(false);
    }

    void OnEnable() {
        
        shipCamera.enabled = true;
        planetCamera.enabled = false;
        computerUI.SetActive(false);
        //sliderUiLogic.computerButton.SetActive(false);
        //sliderUiLogic.backgroundButton.SetActive(false);
    }

    void Update() {
        
        
        if (canSwitch)
        {
            if (!usingAlt && Input.GetKeyDown(KeyCode.E))
            {
            
                usingAlt = true;
                shipCamera.enabled = false;
                planetCamera.enabled = true;
                interactPrompt.SetActive(false);
                computerUI.SetActive(true);
                movementController.enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (usingAlt && Input.GetKeyDown(KeyCode.Escape))
            {
               
                usingAlt = false;
                shipCamera.enabled = true;
                planetCamera.enabled = false;
                interactPrompt.SetActive(true);
                computerUI.SetActive(false);
                movementController.enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    public void SetCanSwitch(bool allow) {
        canSwitch = allow;
    }
}