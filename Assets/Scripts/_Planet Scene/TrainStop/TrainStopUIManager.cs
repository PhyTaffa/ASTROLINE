using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainStopUIManager : MonoBehaviour
{
    public MonoBehaviour movementController;
    public GameObject uiPanel;
    private string targetScene = "ShipMenu";

    public void OnCancel(){
        
        uiPanel.SetActive(false);
        movementController.enabled = true;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnConfirm() {
        SceneManager.LoadScene(targetScene);
        uiPanel.SetActive(false);
        movementController.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}