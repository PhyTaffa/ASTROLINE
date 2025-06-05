using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class ZooObject {
    // exact PlayerPrefs key to check
    public string prefsKey;
    // the GameObject in the scene that should be shown/hidden
    public GameObject objectToSpawn;
}

public class ZooSpawner : MonoBehaviour {
    
    public List<ZooObject> objects;

    void Start(){
        foreach (var z in objects){
            if (z.objectToSpawn == null || String.IsNullOrEmpty(z.prefsKey))
                continue;

            bool shouldAppear = PlayerPrefs.GetInt(z.prefsKey, 0) == 1;
            z.objectToSpawn.SetActive(shouldAppear);
        }
    }

    void Update() {
        
        foreach (var z in objects) {
            if (z.objectToSpawn == null || String.IsNullOrEmpty(z.prefsKey))
                continue;

            bool unlocked = PlayerPrefs.GetInt(z.prefsKey, 0) == 1;
            if (z.objectToSpawn.activeSelf != unlocked)
                z.objectToSpawn.SetActive(unlocked);
        }
    }
}