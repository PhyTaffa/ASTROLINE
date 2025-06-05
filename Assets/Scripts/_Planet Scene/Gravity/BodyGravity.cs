using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BodyGravity : MonoBehaviour {
    
    [SerializeField] private WorldGravity gravity;

    public bool isFlying = false; 
    private Rigidbody rb;
    private Transform tf;

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start() {
        
        if (!rb) 
            rb = GetComponent<Rigidbody>();
        
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        
        tf = transform;
        
        
        if (gravity == null) {
            gravity = GameObject.FindGameObjectWithTag("Planet").GetComponent<WorldGravity>();
        }
    }

    private void Update() {
        if (gravity != null) {
            gravity.Gravity(tf, rb, isFlying); // Pass flag into gravity method
        }
    }
}