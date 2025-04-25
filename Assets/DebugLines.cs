using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLines : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.forward;
        //forward.y = 0f;
        Vector3 right = transform.right;
        //right.y = 0f;
        
        Debug.DrawRay(this.transform.position, forward * 100f, Color.blue);
        Debug.DrawRay(this.transform.position, right * 100f, Color.red);
        
        //Debug.Log($"forward: {forward}");
        //Debug.Log($"right: {right}");
    }
}
