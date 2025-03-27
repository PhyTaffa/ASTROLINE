using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReposition : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Vector3 offset = Vector3.zero;
    
    private Rigidbody playerRB;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        playerRB = GetComponent<Rigidbody>();

        offset.y = boxCollider.size.y / 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))//cahnge it to rb.velocity
        {
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
            { 
                Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * hit.distance, Color.yellow); 
                
                
                
                //change orientation
                transform.up = hit.normal;
                playerRB.velocity = Vector3.zero;
                playerRB.angularVelocity = Vector3.zero;
                
                //reposition
                transform.position = hit.point + new Vector3(hit.normal.x * offset.x, hit.normal.y * offset.y, hit.normal.z * offset.z);
            }
            else
            { 
                Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * 1000, Color.white); 
                
            }
        }

        
    }
}
