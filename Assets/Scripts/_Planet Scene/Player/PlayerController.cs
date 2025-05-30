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
    
    private CameraManager cameraManager;
    
    private void Start(){
        
        rb = GetComponent<Rigidbody>();
        tf = transform;
        //worldGravity = GetComponent<BodyGravity>();
        
        //cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        cinemachineBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);


        // Initialize with the current virtual camera
        if (cinemachineBrain.ActiveVirtualCamera is CinemachineVirtualCamera cam)
            activeCameraTransform = cam.transform;

        cameraManager = FindObjectOfType<CameraManager>();
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
        
        //RotateTowardsMovement();
        // Debug.DrawRay(this.transform.position, this.transform.forward * 10f, Color.green);
        // Debug.DrawRay(this.transform.position, this.transform.right * 10f, Color.yellow);


        //RotateForward();
        
        //new
        // AlignToPlanetSurface();
        // MovePlayer();
        
        // if (input.sqrMagnitude > 0.01f)
        // {
        //     //get the direction to face
        //     Vector3 gravityUp = tf.up;
        //     Vector3 targetForward = Vector3.ProjectOnPlane(input, gravityUp).normalized;
        //
        //     //rotates the character to face it
        //     Quaternion targetRot = Quaternion.LookRotation(targetForward, gravityUp);
        //     tf.rotation = Quaternion.RotateTowards(tf.rotation, targetRot, rotateSpeed * Time.deltaTime);
        //
        //     //move forward based on the character's current forward
        //     // Vector3 move = tf.forward * moveSpeed * Time.deltaTime;
        //     // rb.MovePosition(rb.position + move);
        //     
        //     rb.velocity = tf.forward * moveSpeed;
        // }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(input * (moveSpeed * Time.fixedDeltaTime)));
       /* if (input.sqrMagnitude > 0.01f)
        {
            //get the direction to face
            Vector3 gravityUp = tf.up;
            Vector3 targetForward = Vector3.ProjectOnPlane(input, gravityUp).normalized;

            //rotates the character to face it
            Quaternion targetRot = Quaternion.LookRotation(targetForward, gravityUp);
            tf.rotation = Quaternion.RotateTowards(tf.rotation, targetRot, rotateSpeed * Time.fixedDeltaTime);

            //move forward based on the character's current forward
            // Vector3 move = tf.forward * moveSpeed * Time.fixedDeltaTime;
            // rb.MovePosition(rb.position + move);
            
            rb.velocity = tf.forward * moveSpeed;
        }*/
    }

    private void HandleInputs()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (inputDirection.sqrMagnitude < 0.01f)
        {
            input = Vector3.zero;
            return;
        }

        //Fetches the current camera each frame, quite stoobid 
        //CinemachineVirtualCamera currentCam = cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        
        CinemachineVirtualCameraBase currentCam = cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCameraBase;
        
        if (currentCam == null) return;
        Vector3 cameraForward = currentCam.State.FinalOrientation * Vector3.forward;
        Vector3 cameraRight = currentCam.State.FinalOrientation * Vector3.right;
       
        Vector3 gravityUp = tf.up;

        //fetches the current camera's current forward and right, orthogonally projected onto the character's up: opposite of gravity direction
        Vector3 camForward = Vector3.ProjectOnPlane(cameraForward, gravityUp).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraRight, gravityUp).normalized;
        
        //mashing the input with the cam's vector reference
        Vector3 moveDirWorld = camRight * inputDirection.x + camForward * inputDirection.z;
        
        //this is the magical line that makes it work
        Vector3 moveDirRelative = tf.InverseTransformDirection(moveDirWorld);
        
        //after calculating the alleged good movements, we apply it
        input = moveDirRelative;
        
        
        //most likely add a if that checks if it's in first person or not to disable or not rotation
        if (!cameraManager.IsFirstPersonActive())
        {
            //rotation
            Vector3 flatMoveDir = Vector3.ProjectOnPlane(moveDirWorld, gravityUp).normalized;
            
            if (flatMoveDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(flatMoveDir, gravityUp);
                tf.rotation = Quaternion.RotateTowards(tf.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }
        }

    }
    
    private void RotateTowardsMovement()
    {
        if (input.sqrMagnitude < 0.01f) return;

        Vector3 moveDirWorld = tf.TransformDirection(input);
        Vector3 projectedDir = Vector3.ProjectOnPlane(moveDirWorld, tf.up).normalized;

        // Current forward, projected on the same plane
        Vector3 currentForward = Vector3.ProjectOnPlane(tf.forward, tf.up).normalized;

        // Calculate signed angle between current forward and desired movement dir
        float angle = Vector3.SignedAngle(currentForward, projectedDir, tf.up);

        // Rotate smoothly around the up axis
        float maxStep = rotateSpeed * Time.deltaTime;
        float step = Mathf.Clamp(angle, -maxStep, maxStep);
        tf.Rotate(tf.up, step, Space.World);
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