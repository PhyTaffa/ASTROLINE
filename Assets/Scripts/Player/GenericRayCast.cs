using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GenericRayCast : MonoBehaviour
{
    private GameObject playerGO;
    private Transform playerTransform;
    [SerializeField] private float timer = 2f;
    private float currTimer = 0f;
    private bool isScanAllowed = true;
    
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currTimer = 0f;
        }
        
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Input.GetKey(KeyCode.Q) && isScanAllowed)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            { 
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); 
                //Debug.Log("Did Hit");
                
                currTimer += Time.deltaTime;
                Debug.Log($"{currTimer} time of scanning");
            }
            else
            { 
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white); 
                Debug.Log("Did not Hit"); 
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            isScanAllowed = true;
        }
        

        if (currTimer >= timer)
        {
            Debug.Log("Scan completed");
            currTimer = 0f;
            //isScanAllowed = false;
            StartCoroutine(EnableScan());
        }
        
    }
    
    IEnumerator EnableScan()
    {
        isScanAllowed = false;
        
        float cooldown = timer;
        float currCD = 0f;
        
        currCD += 1f;
        
        if (currCD > cooldown)
        {
            isScanAllowed = true;
            Debug.Log("Scanning Enabled");
        }
        yield return new WaitForSeconds(.1f);;
    }
    
}
