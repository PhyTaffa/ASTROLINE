using System;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class ComputerUIManager : MonoBehaviour {
    
    public GameObject settingsMenu;
    public GameObject upgradeMenu;
    public GameObject FaunaMenu;
    public GameObject FloraMenu;
    
    private void Awake(){
        settingsMenu.SetActive(false);
    }
    void Update(){
        if (settingsMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape)){
            CloseSettings();
        }
    }
    // Settings Buttons
    public void ToggleSettings() {
        bool isOpen = settingsMenu.activeSelf;
        settingsMenu.SetActive(!isOpen);
    }
    public void OpenSettings() {
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
    // Flora Buttons
    // Fauna Buttons
}