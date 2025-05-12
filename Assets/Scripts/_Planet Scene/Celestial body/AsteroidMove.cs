using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMove : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 60f;
    private float rotateSpeed = 500f;

    private Vector3 driftDirection = Vector3.forward;
    private Rigidbody rb;
    private Transform tf;
    private WorldGravity worldGravity;

    private void Start() {

        rb = GetComponent<Rigidbody>();
        tf = transform;
    }
    

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(driftDirection * (moveSpeed * Time.fixedDeltaTime)));
    }


}
