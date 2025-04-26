using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InteractiveWorld : MonoBehaviour {
    
    public Camera planetCamera;
    
    public float rotationSpeed = 5f;  
    
    private bool isDragging = false;
    private bool rotateMode = false;
    private Quaternion initialRotation;
    private Vector3 mouseDownPosition;

    void Start(){
        initialRotation = transform.rotation;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update(){
        
        if (!planetCamera.enabled) return;
        
        if (Input.GetKeyDown(KeyCode.R)){
            
            //switches bool
            rotateMode = !rotateMode;                       
            
            //checks if false
            if (rotateMode){  
                // Cursor.visible = true;
                // Cursor.lockState = CursorLockMode.Confined;
                
            }else{
                
                // Cursor.visible = false;
                // Cursor.lockState = CursorLockMode.Locked;
                // transform.rotation = initialRotation;
            }                                
              
        }

        //checks in true
        if (rotateMode){

            if (Input.GetMouseButtonDown(0)){
                
                isDragging = false;
                mouseDownPosition = Input.mousePosition;
            }
            
            if (Input.GetMouseButton(0)){
                if(Vector3.Distance(mouseDownPosition, Input.mousePosition) > 1f){
                    isDragging = true; // Only set to true if mouse moved significantly
                }
            }
            
            if (Input.GetMouseButtonUp(0)){
                
                if (!isDragging)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("TrainStopUI"))
                    {
                        TrainStop stop = hit.collider.GetComponent<TrainStop>();
                        if (stop != null)
                        {
                            PlayerPrefs.SetString("SpawnPoint", stop.spawnPointName);
                            FadeManager.Instance.FadeToScene("Miguel Testing Gorunds");
                        }
                    }
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
}
