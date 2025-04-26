using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrades : MonoBehaviour{
  
    public GameObject upgrade;
    public TMP_Text buttonLabel;
    public string prefsKey = "UpgradeBlueEnabled";
    private bool isOn;

    void Awake(){
        
        upgrade.SetActive(false);
    }
    
    void Start(){
       
        isOn = (PlayerPrefs.GetInt(prefsKey, 0) == 1);
        upgrade.SetActive(isOn);
        RefreshLabel();
    }
    
    public void Toggle() {

        isOn = !isOn;
        upgrade.SetActive(isOn);
        PlayerPrefs.SetInt(prefsKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
        RefreshLabel();
    }

    private void RefreshLabel() {
        
        if (isOn) {
            buttonLabel.text = "Disable";
        }else {
            buttonLabel.text = "Enable";
        }
    }
}