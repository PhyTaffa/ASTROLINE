using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraTarget : MonoBehaviour
{
    private Transform playerTransform;
    private float speed = 1f;
    private Vector3 targetVector = Vector3.zero;
    private float radius = 0f;
    private float angle = 0f;

    private float thisY = 0f;

    private void Start()
    {
        playerTransform = transform.parent;
        
        targetVector = transform.position - playerTransform.position;
        
        radius = targetVector.magnitude;

        thisY = this.transform.position.y;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            float x = playerTransform.position.x + Mathf.Cos(angle) * radius;
            //float y = playerTransform.position.y;
            float z = playerTransform.position.z + Mathf.Sin(angle) * radius;
            
            transform.position = new Vector3(x, thisY, z);
            
            angle += speed * Time.deltaTime;
        }

    }
}
