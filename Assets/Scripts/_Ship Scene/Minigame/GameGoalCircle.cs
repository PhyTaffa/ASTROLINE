using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGoalCircle : MonoBehaviour{
    
    private void OnTriggerEnter(Collider other){
        
        if (!GameState.IsRunning) return;
        
        if (other.CompareTag("Goal"))
        {
           
            GameGoalSpawner spawner = FindObjectOfType<GameGoalSpawner>();
            if (spawner != null){
                spawner.IncrementScore();
                spawner.NotifyCircleDestroyed();
            }

            Destroy(gameObject);
        }
    }
}
