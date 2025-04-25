using UnityEngine;
using UnityEngine.SceneManagement;

public class NotebookPages : MonoBehaviour
{
    [SerializeField] private GameObject notebookUI;

    private bool isPaused = false;

    void Start() {
        notebookUI.SetActive(false);
    }
    
    void Update(){
        
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.N)){
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (notebookUI != null) {
            notebookUI.SetActive(isPaused);
        }

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }
    
    public void ResumeGame() {
        
        isPaused = false;
        Time.timeScale = 1f;

        if (notebookUI != null)
            notebookUI.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame() {
        
        Debug.Log("QuitGame triggered!");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void GoToMainTitle() {
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start"); // Change this to your main menu scene name
    }
}