using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyGravity : MonoBehaviour
{
    
    [SerializeField] private WorldGravity gravity;

    private Rigidbody rb;
    private Transform tf;
    
    private void Start() {
        
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        tf = transform;

        if (gravity == null)
        {
            gravity = GameObject.FindGameObjectWithTag("World").GetComponent<WorldGravity>();
        }
    }

    private void Update() {
        if (gravity != null)
        {
            gravity.Gravity(tf);
        }
    }
}