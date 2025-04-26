using UnityEngine;

public class UpgradeStateLoader : MonoBehaviour {
    
    public GameObject upgrade;
    public string prefsKey = "UpgradeBlueEnabled";

    void Start() {
        
        bool isOn = PlayerPrefs.GetInt(prefsKey, 0) == 1;
        upgrade.SetActive(isOn);
    }
}