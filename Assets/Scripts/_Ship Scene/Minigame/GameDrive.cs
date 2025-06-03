using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDrive : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float currentSpeed = 0;

    void Update(){

        if (!GameState.IsRunning){
            return;
        }
  
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        
        transform.Translate(0, 0, translation);
        currentSpeed = translation;
        
        transform.Rotate(0, rotation, 0);
    }
}
