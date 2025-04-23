using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour{
    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 5f;

    private Vector3 input;
    private Rigidbody rb;
    private Transform tf;
    private WorldGravity worldGravity;
    
    

    private void Start(){
        
        rb = GetComponent<Rigidbody>();
        tf = transform;
    }

    private void Update(){
        
        HandleInputs();
    }

    private void FixedUpdate(){
        rb.MovePosition(rb.position + transform.TransformDirection(input * moveSpeed * Time.deltaTime));
    }

    private void HandleInputs()
    {
       
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

    }
 
}