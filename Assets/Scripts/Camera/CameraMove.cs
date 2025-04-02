using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject targetToChaseGO = null;
    private Transform targetTransform = null;
    private Vector3 targetPosition = Vector3.zero;

    [SerializeField] private float cameraMoveSpeed = 11f;
    
    //player's info
    private GameObject playerGO = null;
    private Transform playerTransform = null;
    
    void Start()
    {
        targetToChaseGO = GameObject.FindGameObjectWithTag("Camera Target");
        targetTransform = targetToChaseGO.GetComponent<Transform>();

        
        //polayer's info
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGO.GetComponent<Transform>();
        
        //debugg
        // Ensure the camera starts unrotated
        transform.rotation = Quaternion.identity;
        
        
        // Now apply the desired rotation
        //transform.rotation = Quaternion.Euler(20, 0, 0);
    }
    
    void LateUpdate()
    {
        //repositioning the target position
        targetPosition = targetTransform.position;
        
        //this.transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraMoveSpeed);
        this.transform.position = targetPosition;

        
        MoveAround();

        DebugRays();

        LookAtMFPlayer();
    }

    private void LookAtMFPlayer()
    {
        Vector3 direction = new Vector3(playerTransform.position.x - this.transform.position.x, 0, playerTransform.position.z  - this.transform.position.z);
        
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    private void MoveAround()
    {
        
    }

    private void DebugRays()
    {
        //debugg
        // Use the correct forward and right vectors
        Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);  // Visualize forward
        Debug.DrawRay(transform.position, transform.right * 3, Color.red);     // Visualize right

    }
}
