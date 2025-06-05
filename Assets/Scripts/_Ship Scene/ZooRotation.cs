using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZooRotation : MonoBehaviour{
    
    private float speed = 35f;

    void Update(){
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }
}
