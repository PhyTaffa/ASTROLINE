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
    private float initialAngle = 0f;

    private float thisY = 0f;

    private Transform BackOfPlayerTransform = null;
    
    private void Start()
    {
        playerTransform = transform.parent;
        
        targetVector = transform.position - playerTransform.position;
        
        radius = targetVector.magnitude;
        radiusMax = radius;
        radiusMin = radius/5;

        thisY = this.transform.position.y;
        
        angle = Mathf.Atan2(targetVector.z, targetVector.x);
        initialAngle = angle;
        
        //back of player shid
        BackOfPlayerTransform = GameObject.FindGameObjectWithTag("Back of Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(2))
        {
            ResetAngle();
        }

        ZoomTarget();
        
        RotateTargetAroundPlayer();
        //Debug.Log($"ANgle: {angle}");
        //Vector3 direction = new Vector3(playerTransform.position.x - this.transform.position.x, playerTransform.position.y - this.transform.position.y, playerTransform.position.z  - this.transform.position.z);
        
        //this.transform.rotation = Quaternion.LookRotation(direction);
    }

    private void ZoomTarget()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        float zoomSpeed = 10f; 

        radius -= zoomInput * zoomSpeed; 
        radius = Mathf.Clamp(radius, radiusMin, radiusMax);
    }

    private void RotateTargetAroundPlayer()
    {
        float h = Input.GetAxis("Mouse X");
        
        angle += speedRotation * Time.deltaTime * h;
        
        float x = playerTransform.position.x + Mathf.Cos(angle) * radius;
        float z = playerTransform.position.z + Mathf.Sin(angle) * radius;
        
        transform.position = new Vector3(x, thisY, z);
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
}
