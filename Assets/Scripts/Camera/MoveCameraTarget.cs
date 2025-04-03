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
        //use zoom anc clamp it
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        float zoomSpeed = 10f; // Adjust zoom speed as needed

        radius -= zoomInput * zoomSpeed; // Subtract to zoom in, add to zoom out
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
        //hardcoded cuz im dumbdumb, not anymore ඞ
        //angle = 4.7f;
        angle = initialAngle;
    }
}
