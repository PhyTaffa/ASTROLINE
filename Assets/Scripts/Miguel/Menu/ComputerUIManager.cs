using System;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class ComputerUIManager : MonoBehaviour {
    
    public GameObject settingsMenu;
    public GameObject upgradeMenu;
    public GameObject FaunaMenu;
    public GameObject FloraMenu;
    
    public ComputerUISlider sliderUiLogic;
    
    private void Awake(){
        settingsMenu.SetActive(false);
        upgradeMenu.SetActive(false);
        FaunaMenu.SetActive(false);
        FloraMenu.SetActive(false);
    }
    void Update(){
        
        if (!sliderUiLogic.isAtInitial){
            settingsMenu.SetActive(false);
            upgradeMenu.SetActive(false);
            FaunaMenu.SetActive(false);
            FloraMenu.SetActive(false);
            return;
        }
        
        if (settingsMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape)){
            CloseSettings();
        }

        if (upgradeMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape)){
            CloseUpgrades();
        }

        if (FaunaMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape)){
            CloseFauna();
        }

        if (FloraMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape)) {
            CloseFlora();
        }
    }
    // Settings Buttons
    public void ToggleSettings() {
        bool isOpen = settingsMenu.activeSelf;
        upgradeMenu.SetActive(false);
        FaunaMenu.SetActive(false);
        FloraMenu.SetActive(false);
        settingsMenu.SetActive(!isOpen);
    }
    public void OpenSettings() {
        upgradeMenu.SetActive(false);
        FaunaMenu.SetActive(false);
        FloraMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void CloseSettings() {
            settingsMenu.SetActive(false);
    }
    public void TitleScreen() {
        SceneManager.LoadScene("Start");
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    // Upgrade Buttons
    public void ToggleUpgrades() {

        if (!sliderUiLogic.isAtInitial) {
            return;
        }
        
        bool isOpen = upgradeMenu.activeSelf;
        settingsMenu.SetActive(false);
        FaunaMenu.SetActive(false);
        FloraMenu.SetActive(false);
        upgradeMenu.SetActive(!isOpen);
    }
    public void OpenUpgrades() {
        
        if (!sliderUiLogic.isAtInitial) {
            return;
        }
        
        settingsMenu.SetActive(false);
        FaunaMenu.SetActive(false);
        FloraMenu.SetActive(false);
        upgradeMenu.SetActive(true);
    }
    public void CloseUpgrades() {
        upgradeMenu.SetActive(false);
    }
    
    // Flora Buttons
    public void ToggleFlora() {
        
        if (!sliderUiLogic.isAtInitial) {
            return;
        }

        bool isOpen = FloraMenu.activeSelf;
        settingsMenu.SetActive(false);
        upgradeMenu.SetActive(false);
        FaunaMenu.SetActive(false);
        FloraMenu.SetActive(!isOpen);
    }
    public void OpenFlora(){
            
        if (!sliderUiLogic.isAtInitial) {
            return;
        }

        settingsMenu.SetActive(false);
        upgradeMenu.SetActive(false);
        FaunaMenu.SetActive(false);
        FloraMenu.SetActive(true);
    }
    public void CloseFlora() {
        FloraMenu.SetActive(false);
    }
    
    // Fauna Buttons
    public void ToggleFauna() {
            
        if (!sliderUiLogic.isAtInitial) {
            return;
        }

        bool isOpen = FaunaMenu.activeSelf;
        settingsMenu.SetActive(false);
        upgradeMenu.SetActive(false);
        FloraMenu.SetActive(false);
        FaunaMenu.SetActive(!isOpen);
    }
    public void OpenFauna(){
            
        if (!sliderUiLogic.isAtInitial) {
            return;
        }

        settingsMenu.SetActive(false);
        upgradeMenu.SetActive(false);
        FloraMenu.SetActive(false);
        FaunaMenu.SetActive(true);
    }
    public void CloseFauna() {
        FaunaMenu.SetActive(false);
    }
}