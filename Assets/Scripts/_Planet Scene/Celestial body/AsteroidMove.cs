using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMove : MonoBehaviour{
    
    private float minMoveSpeed = 200f;
    private float maxMoveSpeed = 400f;
    
    private float moveSpeed;
    
    private Vector3 driftDirection = Vector3.forward;
    private Rigidbody rb;
    private Transform tf;
    private WorldGravity worldGravity;

    private void Start(){
        
        rb = GetComponent<Rigidbody>();
        tf = transform;

        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + transform.TransformDirection(driftDirection * (moveSpeed * Time.fixedDeltaTime)));
    }
}