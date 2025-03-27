using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject targetToChaseGO = null;
    private Transform targetTransform = null;
    private Vector3 targetPosition = Vector3.zero;

    [SerializeField] private float cameraMoveSpeed = 11f;

    private Vector3 initialForward = Vector3.zero;
    private Vector3 initialRight = Vector3.zero;
    
    void Start()
    {
        targetToChaseGO = GameObject.FindGameObjectWithTag("Camera Target");
        targetTransform = targetToChaseGO.GetComponent<Transform>();

        //debugg
        // Ensure the camera starts unrotated
        transform.rotation = Quaternion.identity;

        // Grab the correct directions while the camera is aligned with world space
        initialForward = transform.forward;
        initialRight = transform.right;

        // Now apply the desired rotation
        transform.rotation = Quaternion.Euler(20, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //repositioning the target position
        targetPosition = targetTransform.position;
        
        this.transform.position = Vector3.Slerp(transform.position, targetPosition, Time.deltaTime * cameraMoveSpeed);
        
        //debugg
        // Use the correct forward and right vectors if needed
        Debug.DrawRay(transform.position, initialForward * 2, Color.blue);  // Visualize forward
        Debug.DrawRay(transform.position, initialRight * 2, Color.red);     // Visualize right
    }
}
