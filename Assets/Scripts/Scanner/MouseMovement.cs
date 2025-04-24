using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 10f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;
   
    private Transform playerTransform; 

    void Start()
    {
        // locking the cursor and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //getting the mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        // Rotation around the x axis (Look up and down)
        xRotation -= mouseY;

        // clamp the rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // Rotation around the y axis (Look up and down)
        yRotation += mouseX;

        // apply the rotation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        
    }
    void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;
    }

}
