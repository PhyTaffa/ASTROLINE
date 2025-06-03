using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MinigameManager : MonoBehaviour{
    
    [Tooltip("First overlay image (click to advance)")]
    [SerializeField] private GameObject image1;
    [Tooltip("Second overlay image (click to advance)")]
    [SerializeField] private GameObject image2;
    
    
    [Header("The UI Button GameObject that starts the minigame")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [Header("The cameras")]
    [SerializeField] private Camera minigameCamera;
    [SerializeField] private Camera cargo1Camera;
    
    [Header("All FX Fires")]
    [SerializeField] private GameObject firesParent;
    
    public GameObject wasdUi;
    public ShipPlayerController movementController; 
    [SerializeField] private ShipCameraManager shipCameraManager;
    [SerializeField] private GameObject gameEffectVolume;
    [SerializeField] private GameObject playUIprompt;
    
    [HideInInspector] public bool hasShownIntro = false;

    private void Awake(){
        minigameCamera.enabled = false;
        image1.SetActive(false);
        image2.SetActive(false);
        firesParent.SetActive(false);
        gameEffectVolume.SetActive(false);
        GameState.IsRunning = false;
    }

    private void Update(){
        
        if (minigameCamera.enabled && !hasShownIntro)
        {
            hasShownIntro = true;
            gameEffectVolume.SetActive(false);
            playUIprompt.SetActive(false);
            if (firesParent != null)
                firesParent.SetActive(false);

            if (image1 != null)
                image1.SetActive(true);
            
            if (image2 != null)
                image2.SetActive(false);
        }

        // if (minigameCamera.enabled && !GameState.IsRunning){
        //     image2.SetActive(true);
        // }
    } 

 
    
    public void OnImage1Clicked(){
            image1.SetActive(false);
            image2.SetActive(true);
            gameEffectVolume.SetActive(false);
    }
    
    
    public void OnQuitClicked(){
        GameState.IsRunning = false;
        Debug.Log("Minigame quit.");
        
      
        minigameCamera.enabled = false;
        cargo1Camera.enabled = true;
        firesParent.SetActive(false);
        image1.SetActive(false);
        image2.SetActive(false);
        playUIprompt.SetActive(true);
        movementController.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        wasdUi.SetActive(true);
        shipCameraManager.ExitAltMode();
                
        hasShownIntro = false;

    }
    
    //Public logic to be used in UI
    public void StartMinigame(){
        gameEffectVolume.SetActive(true);
        playUIprompt.SetActive(false);
        firesParent.SetActive(true);
        image2.SetActive(false);
        GameState.IsRunning = true;
        Debug.Log("Minigame started!");
    }
}
