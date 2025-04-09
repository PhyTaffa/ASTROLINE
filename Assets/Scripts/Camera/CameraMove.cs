using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraMove : MonoBehaviour
{
    //target to chase, childed to player
    private GameObject targetToChaseGO = null;
    private Transform targetTransform = null;

    [SerializeField] private float cameraMoveSpeed = 11f;
    
    //player's info
    private GameObject playerGO = null;
    private Transform playerTransform = null;
    
    void Start()
    {
        targetToChaseGO = GameObject.FindGameObjectWithTag("Camera Target");
        targetTransform = targetToChaseGO.GetComponent<Transform>();
        
        //player's info
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGO.GetComponent<Transform>();
        
        //debugg
        
        // Ensure the camera starts unrotated
        //transform.rotation = Quaternion.identity;
        
        
        // Now apply the desired rotation
        //transform.rotation = Quaternion.Euler(20, 0, 0);
        
    }
    
    void LateUpdate()
    {

        //use this to have the camera ler towards the point childed to the player
        //this.transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraMoveSpeed);
        
        //use this one to avoid weird lerp shenanignas related to the forward and right of the camera, could be changed to be the one of the target so that lerp can lerp
        this.transform.position = targetTransform.position;

        
        LookAtMFPlayer();

        //DebugRays();
    }


    private void LookAtMFPlayer()
    {
        Vector3 direction = new Vector3(
            playerTransform.position.x - this.transform.position.x, 
            playerTransform.position.y - this.transform.position.y, 
            playerTransform.position.z  - this.transform.position.z
            );
        
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    private void DebugRays()
    {
        //debugg
        // Use the correct forward and right vectors
        Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);  // Visualize forward
        Debug.DrawRay(transform.position, transform.right * 3, Color.red);     // Visualize right

    }
}
