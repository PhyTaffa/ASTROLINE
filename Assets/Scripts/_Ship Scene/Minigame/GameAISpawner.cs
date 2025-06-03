using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAISpawner : MonoBehaviour{
    [Header("Prefab of the AI that will keep spawning")]
    [SerializeField] private GameObject aiPrefab;

    [Header("Spawn interval")]
    [SerializeField] private float spawnInterval = 10f;

    [Header("AI will target this")]
    [SerializeField] private Transform playerTransform;
    
    [Header("AI Spawn")]
    [SerializeField] private Transform spawnZoneCenter;

    private void Start(){

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine() {
        
        while (true){

            while (!GameState.IsRunning){
                yield return null;
            }
            
            SpawnOneAI();
            
            while (GameState.IsRunning){
                
                yield return new WaitForSeconds(spawnInterval);
                
                if (GameState.IsRunning){
                    SpawnOneAI();
                }
            }
        }
    }

    private void SpawnOneAI(){
        
        if (aiPrefab == null){
            Debug.LogWarning("AiPrefab is not set.");
            return;
        }

        if (spawnZoneCenter == null){
            Debug.LogWarning("spawnZoneCenter is not set.");
            return;
        }

     
        GameObject newAI = Instantiate(aiPrefab, spawnZoneCenter.position, spawnZoneCenter.rotation);

        // want to force it into Evade mode
        GameAI aiScript = newAI.GetComponent<GameAI>();
        if (aiScript != null) {
          
            aiScript.SetMode(false);
        }

        // safeguarrsd if the prefab GameAI.target was unassigned in the now its playerTransform reference
        if (playerTransform != null && aiScript != null)
        {
            aiScript.target = playerTransform.gameObject;
        }
    }
}
