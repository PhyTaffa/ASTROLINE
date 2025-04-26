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
        
        sliderUiLogic.computerButton.SetActive(false);
        sliderUiLogic.backgroundButton.SetActive(false);
    }

    void Update() {
        
        if (canSwitch && Input.GetKeyDown(KeyCode.E)){
            usingAlt = !usingAlt;
            shipCamera.enabled = !usingAlt;
            planetCamera.enabled = usingAlt;
            interactPrompt.SetActive(!usingAlt && canSwitch);
            computerUI.SetActive(usingAlt);
            movementController.enabled = !usingAlt;
            Cursor.visible = usingAlt;
            Cursor.lockState = usingAlt ? CursorLockMode.None : CursorLockMode.Locked;
            
            sliderUiLogic.computerButton .SetActive(usingAlt);
            sliderUiLogic.backgroundButton.SetActive(usingAlt);
        }
    }
    public void SetCanSwitch(bool allow) {
        canSwitch = allow;
    }
}