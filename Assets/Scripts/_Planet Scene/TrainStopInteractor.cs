using UnityEngine;

public class TrainStopInteractor : MonoBehaviour {
 
    public GameObject trainStopUI;
    public MonoBehaviour movementController;
    
    public static bool TrainStopUIActive { get; private set; }
    
    private bool inZone = false;
    
    
    void Start(){
        trainStopUI.SetActive(false);
        TrainStopUIActive = false;
    }

    void OnTriggerEnter(Collider other){

        if (other.CompareTag("Player")){
            inZone = true;
        }
     
    }

    void OnTriggerExit(Collider other) {
        
        if (other.CompareTag("Player")) {
            inZone = false;
            trainStopUI.SetActive(false);
            //Time.timeScale = 1f;
            movementController.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update(){
        
        if (CameraManager.ScanModeActive || NotebookPages.NotebookOpen){
            return;
        }
           
        // open panel
        if (inZone && !trainStopUI.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            trainStopUI.SetActive(true);
            //Time.timeScale = 0f; 
            movementController.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        
        }
        TrainStopUIActive = trainStopUI.activeSelf;
        
        if (!TrainStopUIActive) {
            Time.timeScale = 1f;
        }else{ 
            Time.timeScale = 0f;
        }
    }
    

}