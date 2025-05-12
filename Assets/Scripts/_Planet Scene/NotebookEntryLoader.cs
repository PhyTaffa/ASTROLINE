using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class NotebookEntry {
    
    // exact key name to toggle the info
    public string prefsKey;
    // the parent hidden for the info of this entry
    public GameObject infoHidder;
}

public class NotebookEntryLoader : MonoBehaviour{
    public List<NotebookEntry> entries;

    void Start() {

        foreach (var e in entries)
            if (e.infoHidder != null && !String.IsNullOrEmpty(e.prefsKey))
                e.infoHidder.SetActive(PlayerPrefs.GetInt(e.prefsKey, 0) == 1);
    }

    void Update() {
  
        foreach (var e in entries)
        {
            if (e.infoHidder == null || String.IsNullOrEmpty(e.prefsKey))
                continue;

            bool unlocked = PlayerPrefs.GetInt(e.prefsKey, 0) == 1;
            if (e.infoHidder.activeSelf != unlocked)
                e.infoHidder.SetActive(unlocked);
        }
    }
}