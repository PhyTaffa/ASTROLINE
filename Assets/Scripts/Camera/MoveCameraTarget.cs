using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraTarget : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private float speedRotation = 15f;
    private Vector3 targetVector = Vector3.zero;
    private float radius = 0f;
    private float radiusMax = 0f;
    private float radiusMin = 0f;
    private float angle = 0f;

    private float thisY = 0f;

    [SerializeField] private float zoomSpeed = 12f;
    private Vector3 targetVectorY = Vector3.zero;
    private Vector3 direction = Vector3.zero;

    private void Start()
    {
        playerTransform = transform.parent;
        
        targetVector =  playerTransform.position - this.transform.position;
        
        radius = targetVector.magnitude;
        radiusMax = radius;
        radiusMin = radius/5;

        thisY = this.transform.position.y;
        
        //sets the angle with the given position
        angle = Mathf.Atan2(targetVector.z, targetVector.x);
    }

    private void LateUpdate()
    {
        //calculates the vector connecting player to target, used in the pitch and orbit
        //direction = new Vector3(playerTransform.position.x - this.transform.position.x, 0, playerTransform.position.z  - this.transform.position.z);

        OrbitTargetAroundPlayer();
        
        //pitches the target so that it's Vector3.right and Vector3.forward are correctly setted.
        PitchTargetToPlayer();
        
        ZoomTarget();
        
        //circular orbit around player's position.
        
        

        if (Input.GetMouseButton(2))
        {
            ResetAngle();
        }

        //Debug
        DrawDirection();
    }
    
        private void OrbitTargetAroundPlayer()
    {
        float h = Input.GetAxis("Mouse X");
        
        angle += speedRotation * Time.deltaTime * h;
        
        float x = playerTransform.position.x + Mathf.Cos(angle) * radius;
        float z = playerTransform.position.z + Mathf.Sin(angle) * radius;
        
        this.transform.position = new Vector3(x, thisY, z);
    }

    private void PitchTargetToPlayer()
    {
        //the y is 0 because this fuckers right and forward is used to change the direction of the player's movement
        Vector3 direction = new Vector3(playerTransform.position.x - this.transform.position.x, 0, playerTransform.position.z  - this.transform.position.z);
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    private void ZoomTarget()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        radius -= zoomInput * zoomSpeed; 
        radius = Mathf.Clamp(radius, radiusMin, radiusMax);
    }


    public void ResetAngle()
    {
        // Get the direction behind the player based on their rotation
        Vector3 backDirection = -playerTransform.forward;

        // Flatten it so Y doesn't affect horizontal angle
        backDirection.y = 0;

        // Normalize it to be safe
        backDirection.Normalize();

        // Calculate the new angle based on the direction behind the player
        angle = Mathf.Atan2(backDirection.z, backDirection.x);
    }
    
    //Debug shids
    private void DrawDirection()
    {
        //visualize direction of walking
        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);  // Visualize forward
        Debug.DrawRay(transform.position, transform.right * 10, Color.red);     // Visualize right
    }

}
