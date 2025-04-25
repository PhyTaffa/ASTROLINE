using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class PlayerController : MonoBehaviour{
    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 5f;

    private Vector3 input;
    private Rigidbody rb;
    private Transform tf;
    private WorldGravity worldGravity;
    
    //movement according to camera
    private Transform activeCameraTransform;
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private Transform tihngThatMoveWithCamera;
    private void Start(){
        
        rb = GetComponent<Rigidbody>();
        tf = transform;
        //worldGravity = GetComponent<BodyGravity>();
        
        //cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        cinemachineBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);


        // Initialize with the current virtual camera
        if (cinemachineBrain.ActiveVirtualCamera is CinemachineVirtualCamera cam)
            activeCameraTransform = cam.transform;
    }

    void OnDestroy()
    {
        cinemachineBrain.m_CameraActivatedEvent.RemoveListener(OnCameraActivated);
    }

    private void OnCameraActivated(ICinemachineCamera fromCam, ICinemachineCamera toCam)
    {
        // if (toCam is CinemachineVirtualCamera newCam)
        // {
        //     activeCameraTransform = newCam.transform;
        // }
    }
    
    private void Update(){
        
        HandleInputs();
        
        
        // Debug.DrawRay(this.transform.position, this.transform.forward * 10f, Color.green);
        // Debug.DrawRay(this.transform.position, this.transform.right * 10f, Color.yellow);


        //RotateForward();
        
        //new
        // AlignToPlanetSurface();
        // MovePlayer();
        
        // if (!isFirstPersonActive && activeCameraTransform != null)
        // {
        //     Vector3 camForward = activeCameraTransform.forward;
        //     Vector3 projectedForward = Vector3.ProjectOnPlane(camForward, tf.up).normalized;
        //
        //     if (projectedForward.sqrMagnitude > 0.001f)
        //     {
        //         Quaternion targetRot = Quaternion.LookRotation(projectedForward, tf.up);
        //         tf.rotation = Quaternion.Slerp(tf.rotation, targetRot, rotateSpeed * Time.deltaTime);
        //     }
        // }

        //Debug.DrawRay(this.transform.position, this.transform.up * 10f, Color.magenta);
        
    }

    private void FixedUpdate(){
        rb.MovePosition(rb.position + transform.TransformDirection(input * (moveSpeed * Time.deltaTime)));
    }

    private void HandleInputs()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        
        CinemachineVirtualCamera currentCam = cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        if (currentCam != null)
        {
            Vector3 gravityUp = tf.up; // or calculate from planet center if you’re not rotating the player yet

            Vector3 camForward = Vector3.ProjectOnPlane(currentCam.transform.forward, gravityUp).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(currentCam.transform.right, gravityUp).normalized;

            Debug.DrawRay(this.transform.position, currentCam.transform.forward * 100f, Color.cyan);
            Debug.DrawRay(this.transform.position, currentCam.transform.right * 100f, Color.magenta);

            //input = inputDirection.x  +  inputDirection.z;
            input = inputDirection;
        }
        else
        {
            input = inputDirection;
        }


        //input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

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