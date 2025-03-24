using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float rotationSpeed = 100.0f;
    [SerializeField] private float jumpForce = 0.1f;

    private Rigidbody rb;
    private GravitationalPull gp;
    
    [SerializeField] private GameObject mainCameraGO;
    private Transform cameraTransform;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gp = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravitationalPull>();
        cameraTransform = mainCameraGO.GetComponent<Transform>();
    }

    void FixedUpdate()
    {

        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * speed;

        //old movement
        
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(rotation, 0, translation);
        
        transform.Rotate(0, rotation, 0);
        
        //new -> get's the camera's forward and right Vector3 and those multiplied by the GetAxis will move the object according to camera
        
        
        //jump
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 force = -gp.GetDirectionOfGravity() * jumpForce;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
