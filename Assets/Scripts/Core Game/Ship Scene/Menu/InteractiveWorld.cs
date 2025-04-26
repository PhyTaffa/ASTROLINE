using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InteractiveWorld : MonoBehaviour {
    
    public Camera planetCamera;
    public ComputerUISlider sliderUiLogic;
    public GameObject confirmDialog;
    
    public float rotationSpeed = 5f;  
    
    private bool isDragging = false;
    private bool rotateMode = false;
    private Quaternion initialRotation;
    private Vector3 mouseDownPosition;
    private TrainStop pendingStop;
    private bool allowRotation = true;
    void Start(){
        initialRotation = transform.rotation;
    }

    void Update(){

        if (!planetCamera.enabled){
            return;
        }
        
        if (!planetCamera.enabled || !allowRotation || confirmDialog.activeSelf)
            return;
        
        bool rotateMode = sliderUiLogic != null && sliderUiLogic.isAtDown && !sliderUiLogic.isMovingPublic;                
        
        if (!rotateMode){
            return;
        }
        
        //checks in true
        if (rotateMode){

            if (Input.GetMouseButtonDown(0)){
                
                isDragging = false;
                mouseDownPosition = Input.mousePosition;
            }
            
            if (Input.GetMouseButton(0)){
                
                // Only set to true if mouse moved significantly
                if(Vector3.Distance(mouseDownPosition, Input.mousePosition) > 1f){
                    isDragging = true; 
                }
            }
            
            if (Input.GetMouseButtonUp(0)){
                
                if (!isDragging)
                {
                    TryBeginTeleport();
                }
                
                isDragging = false;
            }
            
            if (isDragging){

                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                Vector3 rotationVector = new Vector3(mouseY, -mouseX, 0) * rotationSpeed;
                transform.Rotate(rotationVector, Space.World);
            }
        }
    }
    private void TryBeginTeleport()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var hit) && hit.collider.CompareTag("TrainStopUI"))
        {
            var stop = hit.collider.GetComponent<TrainStop>();
            if (stop != null)
            {
                pendingStop = stop;
                ShowConfirm(stop.spawnPointName);
            }
        }
    }
    
    private void ShowConfirm(string stopName) {
        confirmDialog.SetActive(true);
    }

    public void ConfirmTeleport() {
       
        PlayerPrefs.SetString("SpawnPoint", pendingStop.spawnPointName); 
        FadeManager.Instance.FadeToScene("Miguel Testing Gorunds");
        confirmDialog.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.enabled = false;
        allowRotation = false;  
    }

    public void CancelTeleport()
    {
        CloseConfirm();
    }

    private void CloseConfirm()
    {
        pendingStop = null;
        confirmDialog.SetActive(false);
    }
    
}
