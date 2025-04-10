using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveWorld : MonoBehaviour {
    
    public float rotationSpeed = 5f;  
    
    private bool isDragging = false;
    private bool rotateMode = false;
    private Quaternion initialRotation;

    void Start(){
        initialRotation = transform.rotation;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update(){
        
        if (Input.GetKeyDown(KeyCode.R)){
            
            //switches bool
            rotateMode = !rotateMode;                       
            
            //checks if false
            if (rotateMode){  
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                
            }else{
                
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                transform.rotation = initialRotation;
            }                                
              
        }

        //checks in true
        if (rotateMode){

            if (Input.GetMouseButtonDown(0)){
                
                isDragging = true; 
            }

            if (Input.GetMouseButtonUp(0)){
                
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
