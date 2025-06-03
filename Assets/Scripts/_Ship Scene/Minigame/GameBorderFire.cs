using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBorderFire : MonoBehaviour{
    
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Goal")){
            Destroy(other.gameObject);
            Debug.Log("MEGA sus");
        }
        if (other.CompareTag("Avatar")){
            GameState.IsRunning = false;
            Debug.Log("sus");
        }
    }
}
