using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMove : MonoBehaviour{
    
    private float moveSpeed = 40f;
    
    private Vector3 driftDirection = Vector3.forward;
    private Rigidbody rb;
    private Transform tf;
    private WorldGravity worldGravity;

    private void Start(){
        
        rb = GetComponent<Rigidbody>();
        tf = transform;
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + transform.TransformDirection(driftDirection * (moveSpeed * Time.fixedDeltaTime)));
    }
}