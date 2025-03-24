using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalPull : MonoBehaviour
{
    private Vector3 centerOfGravity;
    private GameObject playerGO;
    private Transform playerTransform;
    private Rigidbody playerRB;
    
    [SerializeField] private float gravityForce = 100f;
    [SerializeField] float rotationSpeed = 5f;
    
    private Vector3 directionOfGravity = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;
    private void Awake()
    {
        centerOfGravity = this.transform.position;
        
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerGO.GetComponent<Transform>();
        playerRB = playerGO.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        directionOfGravity = (centerOfGravity - playerTransform.position).normalized;
        
        playerRB.AddForce(directionOfGravity * gravityForce, ForceMode.Force);
        
        targetRotation = Quaternion.FromToRotation(playerTransform.up, -directionOfGravity) * playerTransform.rotation;
        
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // show direction of attraction
        Debug.DrawLine(playerTransform.position, playerTransform.position + directionOfGravity * 2, Color.magenta);
    }

    private void Update()
    {
        //make player stand up
        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerTransform.rotation = targetRotation;
        }
    }

    public Vector3 GetDirectionOfGravity()
    {
        return directionOfGravity;
    }
}
