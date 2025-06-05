using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameGoalSpawner : MonoBehaviour{
    
    [Header("Prefab of the circle to spawn")]
    [SerializeField] private GameObject circlePrefab;
    
    [Header("UI Text to display score")]
    [SerializeField] private TextMeshProUGUI  scoreText;
    
    private float spawnInterval = 6f;
    private BoxCollider spawnZone;
    private int score = 0;

    public static bool scoreReachedFive = false;
    
    private void Awake(){
        
        spawnZone = GetComponent<BoxCollider>();
    }

    private void Start() {
       
        StartCoroutine(SpawnRoutine());
        UpdateScoreUI();
    }
    
    private void Update(){
        
        scoreText.gameObject.SetActive(GameState.IsRunning);
        
        if (!GameState.IsRunning){
            score = 0;
            UpdateScoreUI();
            
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.I)){
            scoreReachedFive = false;
        }
    }
    private IEnumerator SpawnRoutine(){
        
        while (!GameState.IsRunning){
            yield return null;
        }
        
        SpawnCircle();
        
        yield return new WaitForSeconds(spawnInterval);
        
        while (true){
            
            if (GameState.IsRunning){
                SpawnCircle();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCircle(){
        
        if (!GameState.IsRunning) return;
        if (circlePrefab == null) return;

        Vector3 center = spawnZone.center + transform.position;
        Vector3 size = spawnZone.size;
        
        float randomX = Random.Range(-size.x / 2f, size.x / 2f);
        float minY = 0;
        float randomZ = Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = center + new Vector3(randomX, minY, randomZ);
        
        Instantiate(circlePrefab, spawnPos, Quaternion.identity);
    }

    public void IncrementScore(){
        if (!GameState.IsRunning) return;
        score++;
        UpdateScoreUI();
        
        if (score >= 5){
            scoreReachedFive = true;
        }
    }
    
    private void UpdateScoreUI(){
        
        if (scoreText != null){
            scoreText.text = "Score: " + score;
        }
    }
}