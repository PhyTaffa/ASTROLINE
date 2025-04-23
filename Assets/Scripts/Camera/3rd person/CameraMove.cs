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
    
    private Vector3 direction = Vector3.zero;

    private float hitValue = 0f;
    public float HitValue
    {
        get { return hitValue; }
        set { hitValue = value; }
    }
    private bool isCurrentlyOccluded = false;

    public bool IsCurrentlyOccluded
    {
        get { return isCurrentlyOccluded; }
        set { isCurrentlyOccluded = value; }
    }
    private Vector3 hitPosition = Vector3.zero;

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
        CheckForOcclusion();
        //float a = ObjectOcclusion();
        
        //DebugRays();
    }

    /// <summary>
    /// Used in the MoveCameraTarget; as the name suggests; it's used for the object occlusion 
    /// </summary>
    /// <returns>distance value that will be used in the radius of orbit of camera</returns>
    // public float ObjectOcclusion()
    // {
    //     
    //     //using the directoin vector3 and this posiiont i cast the ray
    //     RaycastHit hit;
    //     // Does the ray intersect any objects excluding the player layer
    //     if (Physics.Raycast(playerTransform.position, -direction, out hit, direction.magnitude))
    //     {
    //         Debug.DrawRay(playerTransform.position, -direction, Color.yellow);
    //         //Debug.Log($"Hit name: {hit.collider.name}");
    //         return hit.distance;
    //     }
    //     else
    //     {
    //         Debug.DrawRay(playerTransform.position, -direction, Color.white);
    //         //Debug.Log("Did not Hit");
    //     }
    //     
    //     return -1f;
    // }
    
    // public bool IsOccluded()
    // {
    //     RaycastHit hit;
    //     Vector3 direction = transform.position - playerTransform.position;
    //
    //     if (Physics.Raycast(playerTransform.position, -direction, out hit, direction.magnitude))
    //     {
    //         Debug.DrawRay(playerTransform.position, -direction, Color.yellow);
    //         //Debug.Log($"Hit name: {hit.collider.name}");
    //         hitPosition = hit.point;
    //         
    //         hitValue = hit.distance;
    //         HitValue = hitValue;
    //         
    //         return true;
    //     }
    //     else
    //     {
    //         Debug.DrawRay(playerTransform.position, -direction, Color.white);
    //         hitValue = -1f;
    //         HitValue = hitValue;
    //         //Debug.Log("Did not Hit");
    //     }
    //     return false;
    // }

    private void CheckForOcclusion()
    {
        RaycastHit hit;
        direction = transform.position - playerTransform.position;

        if (Physics.Raycast(playerTransform.position, direction, out hit, direction.magnitude))
        {
            //occluded
            if (!hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(playerTransform.position, direction, Color.yellow);
                hitPosition = hit.point;
                HitValue = hit.distance;
                IsCurrentlyOccluded = true;
                return;
            }
        }

        Debug.DrawRay(playerTransform.position, direction, Color.white);
        HitValue = -1f;
        IsCurrentlyOccluded = false;
    }

    private void LookAtMFPlayer()
    {
        direction = new Vector3(
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
        //Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);  // Visualize forward
        //Debug.DrawRay(transform.position, transform.right * 3, Color.red);     // Visualize right

        Debug.DrawRay(transform.position, playerTransform.position - transform.position, Color.green);
    }
}
