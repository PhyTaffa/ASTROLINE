using UnityEngine;
using UnityEngine.SceneManagement;

public class NotebookPages : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject notebookUI;
    [SerializeField] private GameObject settingsPage;
    [SerializeField] private GameObject floraPage;
    [SerializeField] private GameObject faunaPage;
    
    public static bool NotebookOpen { get; private set; }
    
    private bool isPaused = false;
    private enum NotebookState { Settings, Flora, Fauna }
    private NotebookState currentState = NotebookState.Settings;
    
    void Start() {
        notebookUI.SetActive(false);
        settingsPage.SetActive(false);
        floraPage.SetActive(false);
        faunaPage.SetActive(false);
        NotebookOpen = false;
    }

    void Update(){

        if (CameraManager.ScanModeActive || TrainStopInteractor.TrainStopUIActive){
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.N)){
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        NotebookOpen  = isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        
        notebookUI.SetActive(isPaused);
        
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;

        if (isPaused) {
            SwitchToPage(NotebookState.Settings); 
        }
            
    }
    
    public void ResumeGame() {
        
        isPaused = false;
        NotebookOpen  = false;
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
        SceneManager.LoadScene("Start"); 
    }
    private void SwitchToPage(NotebookState newState) {
        
        settingsPage.SetActive(newState == NotebookState.Settings);
        floraPage.SetActive(newState == NotebookState.Flora);
        faunaPage.SetActive(newState == NotebookState.Fauna);
        currentState = newState;
    }
    public void OpenSettingsPage() => SwitchToPage(NotebookState.Settings);
    public void OpenFloraPage() => SwitchToPage(NotebookState.Flora);
    public void OpenFaunaPage() => SwitchToPage(NotebookState.Fauna);
}