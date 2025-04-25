using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    [Tooltip("The thing you actually want to enable/disable when this button is clicked.")]
    public GameObject upgradeObject;

    [Tooltip("Optional: the text label on the button, so we can show ON/OFF.")]
    public Text buttonLabel;

    private bool isOn;

    void Start()
    {
        // initialize to match the actual state
        isOn = upgradeObject != null && upgradeObject.activeSelf;
        RefreshLabel();
    }

    /// <summary>
    /// Hook this up to the Button.onClick → UpgradeToggle.Toggle() 
    /// </summary>
    public void Toggle()
    {
        if (upgradeObject == null) return;

        isOn = !isOn;
        upgradeObject.SetActive(isOn);
        RefreshLabel();
    }

    private void RefreshLabel()
    {
        if (buttonLabel == null) return;
        buttonLabel.text = isOn ? "Disable Upgrade" : "Enable Upgrade";
    }
}