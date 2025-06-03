using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereRotation : MonoBehaviour{
    
    [SerializeField] private Transform sun;
    private void Update() {
        
        Vector3 dir = sun.position - transform.position;
        Quaternion rotateUpToTarget = Quaternion.FromToRotation(transform.up, dir.normalized);
        transform.rotation = rotateUpToTarget * transform.rotation;
    }
}
