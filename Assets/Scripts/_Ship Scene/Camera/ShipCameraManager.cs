using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCameraManager : MonoBehaviour {
    public Camera shipCamera;
    public Camera planetCamera;
    public Camera cargo1Camera;
    public Camera cargo2Camera;
    public Camera gameCamera;
    public GameObject shipInteractPrompt; 
    public GameObject cargoInteractPrompt; 
    public GameObject computerUI;
    public ShipPlayerController movementController; 
    public ComputerUISlider sliderUiLogic;
    public GameObject wasdUi;
    
    public ComputerUIManager uiManager;
    
    private bool usingAlt = false;
    
    private bool canSwitchShipZone  = false;
    private bool canSwitchCargoZone = false;


    private void Awake() {
        
        computerUI.SetActive(false);
    }

    void OnEnable() {
        
        shipCamera.enabled = true;
        planetCamera.enabled = false;
        gameCamera.enabled = false;
        cargo1Camera.enabled = false;
        cargo2Camera.enabled = false;
        
        computerUI.SetActive(false);
        //sliderUiLogic.computerButton.SetActive(false);
        //sliderUiLogic.backgroundButton.SetActive(false);
        wasdUi.SetActive(true);
    }

    private void Update() 
    {
        // If we’re currently NOT in any alternate mode, check for E in the zones:
        if (!usingAlt) 
        {
            // 1) Ship‐computer zone: pressing E → show planetCamera
            if (canSwitchShipZone && Input.GetKeyDown(KeyCode.E)) 
            {
                EnterAltMode(AltMode.ShipComputer);
                return;
            }

            // 2) Cargo zone: pressing E → show cargo1Camera
            if (canSwitchCargoZone && Input.GetKeyDown(KeyCode.E)) 
            {
                EnterAltMode(AltMode.CargoView);
                return;
            }
        }
        else 
        {
            // If we ARE in an alternate mode, pressing Escape will revert to shipCamera
            // but only if no sub‐menu is open:
            if (currentAltMode == AltMode.ShipComputer && Input.GetKeyDown(KeyCode.Escape)) 
            {
                // If any UI submenu is up, ignore Escape
                if (uiManager.settingsMenu.activeSelf  ||
                    uiManager.upgradeMenu.activeSelf    ||
                    uiManager.FaunaMenu.activeSelf      ||
                    uiManager.FloraMenu.activeSelf) 
                {
                    return;
                }

                ExitAltMode();
                return;
            }
        }
    }

    // ────────────────────────────────────────────────────────────────────────────
    // These two methods will be called by PlayerButtonDetector when
    // the player enters/leaves each zone.

    public void SetShipSwitch(bool allow) 
    {
        canSwitchShipZone = allow;

        // Show/hide the ship‐interact prompt:
        shipInteractPrompt.SetActive(allow && !usingAlt);
    }

    public void SetCargoSwitch(bool allow) 
    {
        canSwitchCargoZone = allow;

        // Show/hide the cargo‐interact prompt:
        cargoInteractPrompt.SetActive(allow && !usingAlt);
    }

    // ────────────────────────────────────────────────────────────────────────────
    // Internally, track which alternate mode we’re in:
    private enum AltMode 
    {
        None,
        ShipComputer, // planetCamera
        CargoView     // cargo1Camera
    }
    private AltMode currentAltMode = AltMode.None;

    private void EnterAltMode(AltMode mode) 
    {
        // Disable whichever camera was previously active
        switch (currentAltMode) 
        {
            case AltMode.ShipComputer:
                planetCamera.enabled = false;
                break;
            case AltMode.CargoView:
                gameCamera.enabled = false;
                break;
        }

        // Now enable the correct new camera
        switch (mode) 
        {
            case AltMode.ShipComputer:
                // Turn off ship’s main camera, turn on planetCamera
                shipCamera.enabled = false;
                planetCamera.enabled = true;
                cargo1Camera.enabled = false;
                gameCamera.enabled = false;
                computerUI.SetActive(true);
                
                break;

            case AltMode.CargoView:
                // Turn off ship’s main camera, turn on cargo1Camera:
                shipCamera.enabled = false;
                planetCamera.enabled = false;
                cargo1Camera.enabled = false;
                gameCamera.enabled = true;
                break;
        }

        // Hide both prompts
        shipInteractPrompt.SetActive(false);
        cargoInteractPrompt.SetActive(false);

        // Show “computer” UI overlay
        

        // Disable player movement
        movementController.enabled = false;

        // Show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Hide WASD hints
        wasdUi.SetActive(false);

        usingAlt = true;
        currentAltMode = mode;
    }

    public void ExitAltMode(){
        // Turn off whichever alternate camera was on
        switch (currentAltMode) 
        {
            case AltMode.ShipComputer:
                planetCamera.enabled = false;
                shipCamera.enabled = true;

                break;
            case AltMode.CargoView:
                gameCamera.enabled = false;
                cargo1Camera.enabled = true;

                break;
        }
        

        // Hide the computer UI
        computerUI.SetActive(false);

        // Re‐enable player movement
        movementController.enabled = true;

        // Hide cursor & lock
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Show WASD hints again
        wasdUi.SetActive(true);

        // Re‐show whichever prompt is valid (if still in that zone)
        if (canSwitchShipZone)  
            shipInteractPrompt.SetActive(true);
        if (canSwitchCargoZone) 
            cargoInteractPrompt.SetActive(true);

        usingAlt = false;
        currentAltMode = AltMode.None;
    }
}