using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        // Gets the saved spawn point name
        string spawnPointName = PlayerPrefs.GetString("SpawnPoint", "");

        if (spawnPointName != "")
        {
            // Finds the spawn point in the scene
            GameObject spawnPoint = GameObject.Find(spawnPointName);

            if (spawnPoint != null)
            {
                // Moves player instantly to spawn point position
                transform.position = spawnPoint.transform.position;
                transform.rotation = spawnPoint.transform.rotation;
            }
            else
                Debug.LogError("Couldn't find Spawn Point: " + spawnPointName);
        }
        else
            Debug.LogError("SpawnPoint was never set!");
    }
}