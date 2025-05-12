using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpawn : MonoBehaviour{
    void Start() {
      
        string spawnName = PlayerPrefs.GetString("SpawnPoint", "");
        if (string.IsNullOrEmpty(spawnName)){
            
            Debug.LogWarning("No SpawnPoint set – using default transform");
            return;
        }

        GameObject spawnpoint = GameObject.Find(spawnName);
        Vector3 finalPos = spawnpoint.transform.position;
        Quaternion finalRot = spawnpoint.transform.rotation;

        if (spawnName == "TrainStopCheckPoint North"){
            finalPos += Vector3.forward * 2f;
        }
        else if (spawnName == "TrainStopCheckPoint South"){
            finalPos += Vector3.back * 2f;
        }
        else if (spawnName == "TrainStopCheckPoint East"){
            finalPos += Vector3.right * 2f;
        }
        else if (spawnName == "TrainStopCheckPoint West"){
            finalPos += Vector3.left * 2f;
        }
        else if (spawnName == "TrainStopCheckPoint CenterFront"){
            finalPos += Vector3.forward * 2f;
        }
        else if (spawnName == "TrainStopCheckPoint CenterBack"){
            finalPos += Vector3.back * 2f;
        }
        
        transform.position = finalPos;
        transform.rotation = finalRot;
    }
}