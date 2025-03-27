using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 100.0f;
    [SerializeField] private float jumpForce = 10f;

    private Rigidbody rb;
    private GravitationalPull gp;
    
    //movement according to camera
    [SerializeField] private GameObject cameraRefGO = null;
    private Transform cameraTransform;
    private GameObject cameraPlaneGO = null;
    
    //saving directions, excessive
    private float rawZDirection = 0f;
    private float rawXDirection = 0f;
    private Vector3 processedXDirection = Vector3.zero;
    private Vector3 processedZDirection = Vector3.zero;
    private Vector3 proccessInput = Vector3.zero;
    private Vector3 cameraRight = Vector3.zero;
    private Vector3 cameraForward = Vector3.zero;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        cameraTransform = cameraRefGO.GetComponent<Transform>();
        cameraRight = cameraTransform.right;
        cameraForward = cameraTransform.forward;
    }

    void FixedUpdate()
    {
        MovementAccordingToCamera();
        jump();
    }

    private void jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Vector3 force = -gp.GetDirectionOfGravity() * jumpForce;
            //rb.AddForce(force, ForceMode.Impulse);
        }
    }

    private void MovementAccordingToCamera()
    {
        rawZDirection = Input.GetAxis("Vertical") * Time.fixedDeltaTime;
        rawXDirection = Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
        
        //amongus
        processedXDirection = rawXDirection * cameraRight;
        processedZDirection = rawZDirection * cameraForward;
        Vector3 processedDirection = (processedXDirection + processedZDirection) * speed;
        
        proccessInput = processedDirection;

        transform.position += proccessInput;
        //rb.velocity = proccessInput;
        //rb.AddForce(proccessInput, ForceMode.Force);
    }
}
