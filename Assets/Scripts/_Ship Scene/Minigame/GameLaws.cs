using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLaws : MonoBehaviour{
    // keep track of the last-known state to detect "OnExit" from running to not running
    private bool _lastIsRunning = false;
    
    [Header("Spawners objects")]
    public Transform avatarSpawn;
    public Transform deathSpawn;

    [Header("Avatar and Death objects")]
    public GameObject avatar;
    public GameObject deathObject;
    
    [SerializeField] private MinigameManager minigameManager;
    
    private void Awake(){

        GameState.IsRunning = false;
        _lastIsRunning = GameState.IsRunning;
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.R)) {
            
            if (!GameState.IsRunning) {
                GameState.IsRunning = true;
                Debug.Log("Game started!");
                
            }else {
                GameState.IsRunning = false;
                Debug.Log("Game stopped!");
            }
        }

      
        if (_lastIsRunning && !GameState.IsRunning){
       
            if (avatar != null && avatarSpawn != null){
                Rigidbody avatarRb = avatar.GetComponent<Rigidbody>();
                avatarRb.velocity = Vector3.zero;
                avatarRb.angularVelocity = Vector3.zero;
                avatar.transform.position = avatarSpawn.position;
                avatar.transform.rotation = avatarSpawn.rotation;
            }

            if (deathObject != null && deathSpawn != null){
                deathObject.transform.position = deathSpawn.position;
                deathObject.transform.rotation = deathSpawn.rotation;
            }
            
            CleanupAllSpawned();
        }

        _lastIsRunning = GameState.IsRunning;
    }

    private void CleanupAllSpawned(){
        
        // find all GameObjects in the scene with tag spawned and kill them
        minigameManager.hasShownIntro = false;
        GameObject[] allSpawned = GameObject.FindGameObjectsWithTag("Spawned");
        for (int i = 0; i < allSpawned.Length; i++){
            Destroy(allSpawned[i]);
        }
    }
    
    // //Public logic to be used in UI
    // public void StartMiniGame(){
    //     GameState.IsRunning = true;
    // }
}