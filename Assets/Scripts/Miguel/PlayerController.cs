using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 5f;

    private Vector3 input;
    private Rigidbody rb;
    private Transform tf;
    private WorldGravity worldGravity;

    private void Start(){
        
        rb = GetComponent<Rigidbody>();
        tf = transform;
        //worldGravity = GetComponent<BodyGravity>();
        
    }

    private void Update(){
        
        HandleInputs();
        
        //RotateForward();
        
        //new
        // AlignToPlanetSurface();
        // MovePlayer();
    }

    private void FixedUpdate(){
        rb.MovePosition(rb.position + transform.TransformDirection(input * moveSpeed * Time.deltaTime));
    }

    private void HandleInputs()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

    }
    //
    // private void AlignToPlanetSurface()
    // {
    //     if (worldGravity == null) return;
    //
    //     // Get gravity direction (out from planet center)
    //     Vector3 gravityDir = (tf.position - worldGravity.transform.position).normalized;
    //
    //     // Align player "up" to gravity direction
    //     Quaternion targetRot = Quaternion.FromToRotation(tf.up, gravityDir) * tf.rotation;
    //     tf.rotation = Quaternion.Slerp(tf.rotation, targetRot, rotateSpeed * Time.deltaTime);
    // }
    //
    // private void MovePlayer()
    // {
    //     if (input == Vector3.zero) return;
    //
    //     // Move relative to planet surface
    //     Vector3 move = tf.TransformDirection(input) * moveSpeed * Time.deltaTime;
    //     tf.position += move;
    //
    //     // Rotate mesh toward movement
    //     if (model != null)
    //     {
    //         Quaternion faceDir = Quaternion.LookRotation(tf.TransformDirection(input), tf.up);
    //         model.rotation = Quaternion.Slerp(model.rotation, faceDir, rotateSpeed * Time.deltaTime);
    //     }
    // }
}