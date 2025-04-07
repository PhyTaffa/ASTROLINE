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
    //private float rawZDirection = 0f;
    //private float rawXDirection = 0f;
    //private Vector3 processedXDirection = Vector3.zero;
    //private Vector3 processedZDirection = Vector3.zero;
    //private Vector3 cameraRight = Vector3.zero;
    //private Vector3 cameraForward = Vector3.zero;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        //for simplicity should also metch the ackrual camera GO
        cameraTransform = cameraRefGO.GetComponent<Transform>();
        //cameraRight = cameraTransform.right;
        //cameraForward = cameraTransform.forward;
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
        float rawZDirection = Input.GetAxis("Vertical");
        float rawXDirection = Input.GetAxis("Horizontal");
        
        float deltaTimedXDirection = rawXDirection * Time.fixedDeltaTime;
        float deltaTimedZDirection = rawZDirection * Time.fixedDeltaTime;
        
        //Merging all the direction
        Vector3 processedXDirection = rawXDirection * cameraTransform.right;
        Vector3 processedZDirection = rawZDirection * cameraTransform.forward;
        Vector3 processedDirection = (processedXDirection + processedZDirection) * speed;
        
        //acktaul moving
        transform.position += processedDirection;
        //rb.velocity = proccessInput;
        //rb.AddForce(proccessInput, ForceMode.Force);
    }
}
