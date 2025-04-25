using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraTarget : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private float speedRotation = 15f;
    private Vector3 targetVector = Vector3.zero;
    private float radius = 0f;
    private float radiusMax = 0f;
    private float radiusMin = 0f;
    private float angle = 0f;

    private float thisY = 0f;

    private float radiusPreviousToHit = 0f;

    [SerializeField] private float zoomSpeed = 12f;
    private Vector3 targetVectorY = Vector3.zero;
    private Vector3 direction = Vector3.zero;
    
    [SerializeField] private GameObject thirdPersonCamera = null;
    private CameraMove cameraMove = null;
    
    private float radiusZoomed = 0f;
    private bool wasOccludedLastFrame = false;
    [SerializeField] private float radiusLerpSpeed = 0f;
    
    private int framesClear = 0;
    [SerializeField] private int requiredClearFrames = 5;

    private void Start()
    {
        playerTransform = transform.parent;
        targetVector = playerTransform.position - transform.position;

        radius = targetVector.magnitude;
        radiusZoomed = radius; // set the initial zoomed radius to current radius
        radiusMax = radius;
        radiusMin = radius / 5;

        thisY = transform.position.y;
        angle = Mathf.Atan2(targetVector.z, targetVector.x);
        cameraMove = FindObjectOfType<CameraMove>();
    }

    private void LateUpdate()
    {
        //calculates the vector connecting player to target, used in the pitch and orbit
        
        //direction = new Vector3(playerTransform.position.x - this.transform.position.x, 0, playerTransform.position.z  - this.transform.position.z);
        
        ZoomTarget();
        
        OrbitTargetAroundPlayer();
        
        //pitches the target so that it's Vector3.right and Vector3.forward are correctly setted.
        PitchTargetToPlayer();
        
        
        
        
        if (Input.GetMouseButton(2))
        {
            ResetAngle();
        }

        //Debug
        DrawDirection();
    }
    
    private void OrbitTargetAroundPlayer()
    {
         # region orbit on a plane
         // float h = Input.GetAxis("Mouse X");
         //
         // angle += speedRotation * Time.deltaTime * h;
         //
         // float x = playerTransform.position.x + Mathf.Cos(angle) * radius;
         // float z = playerTransform.position.z + Mathf.Sin(angle) * radius;
         //
         // //thisY = playerTransform.up.y * 4f;
         // thisY = playerTransform.up.y * 4f + playerTransform.position.y;
         //
         // this.transform.position = new Vector3(x, thisY, z);
         # endregion


        #region no clue
            //got no clue
            // Vector3 localUp = playerTransform.up;
            // Vector3 offset = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, localUp) * (transform.position - playerTransform.position).normalized * radius;
            // transform.position = playerTransform.position + offset;
            //
            // transform.LookAt(playerTransform.position, localUp);
        #endregion

        float h = Input.GetAxis("Mouse X");
        angle += speedRotation * Time.deltaTime * h;

        // The up vector is the "north pole" at the player's position on the sphere
        Vector3 localUp = playerTransform.up;

        // Create a rotation around the local up axis
        Quaternion rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, localUp);

        // Get a base offset direction — use any perpendicular vector to localUp
        Vector3 baseDirection = Vector3.Cross(localUp, playerTransform.right).normalized;

        // Orbit offset is a rotated version of the base direction
        Vector3 orbitOffset = rotation * baseDirection * radius;

        // Final camera position is orbit position + height offset along up
        Vector3 heightOffset = localUp * 4f; // adjust height as needed
        transform.position = playerTransform.position + orbitOffset + heightOffset;

        // Rotate the target according to the player's up vector
        transform.rotation = Quaternion.LookRotation(playerTransform.position - transform.position, localUp);

    }

    private void PitchTargetToPlayer()
    {
        //the y is 0 because this fuckers right and forward is used to change the direction of the player's movement
        Vector3 direction = new Vector3(playerTransform.position.x - this.transform.position.x, 0, playerTransform.position.z  - this.transform.position.z);
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    /// <summary>
    /// Gets the input of the mouse wheel ans chges it's value according to the direction and clamps it, RADIUS which is later used to define the posiiton of the target
    /// </summary>
    private void ZoomTarget()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        radiusZoomed -= zoomInput * zoomSpeed;
        radiusZoomed = Mathf.Clamp(radiusZoomed, radiusMin, radiusMax);

        float objectOccRadius = cameraMove.HitValue;
        bool isOccludedThisFrame = cameraMove.IsCurrentlyOccluded;
        
        if (isOccludedThisFrame)
        {
            radius = Mathf.Min(radiusZoomed, (objectOccRadius - 0.1f));
        }
        else
        {
            //positon prediciton
            float x = playerTransform.position.x + Mathf.Cos(angle) * radiusZoomed;
            float z = playerTransform.position.z + Mathf.Sin(angle) * radiusZoomed;
            Vector3 predictedPos = new Vector3(x, thisY, z);

            
            //Raycast on prediciotn
            Vector3 rayDir = predictedPos - playerTransform.position;
            float rayLength = rayDir.magnitude;
            rayDir.Normalize();

            RaycastHit hit;
            bool willBeOccluded = Physics.Raycast(playerTransform.position, rayDir, out hit, rayLength);

            Debug.DrawRay(playerTransform.position, rayDir * rayLength, Color.cyan);
            
            //if (!hit.collider.gameObject.CompareTag("Player") )
            //{
                //Debug.Log($"hit name : {hit.collider.gameObject.name}");
            //}
            if (!willBeOccluded)
            {
                 radius = Mathf.Lerp(radius, radiusZoomed, Time.deltaTime * radiusLerpSpeed);
            }
            else
            {
                radius = Mathf.Min(radius, hit.distance - 0.1f); // buffer to reduce jitter
            }
        }
        radius = Mathf.Clamp(radius, radiusMin, radiusMax);
    }

    private void ResetAngle()
    {
        // Get the direction behind the player based on their rotation
        Vector3 backDirection = -playerTransform.forward;

        // Flatten it so Y doesn't affect horizontal angle
        backDirection.y = 0;

        // Normalize it to be safe
        backDirection.Normalize();

        // Calculate the new angle based on the direction behind the player
        angle = Mathf.Atan2(backDirection.z, backDirection.x);
    }
    
    //Debug shids
    private void DrawDirection()
    {
        //visualize direction of walking
        Debug.DrawRay(transform.position, playerTransform.forward * 1000, Color.blue);  // Visualize forward
        Debug.DrawRay(transform.position, playerTransform.right * 1000, Color.red);     // Visualize right
        
        
    }

}
