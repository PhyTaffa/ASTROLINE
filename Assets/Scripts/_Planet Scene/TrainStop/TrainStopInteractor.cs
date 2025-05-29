using System.Collections;
using UnityEngine;

public class TrainStopInteractor : MonoBehaviour {
    
    public GameObject warningsUI;
    public GameObject trainStopUI;
    public GameObject worldInstuctionsUI;
    
    private float worldFadeDelay = 0.5f;
    private float worldFadeDuration = 1f;
    private Coroutine worldFadeRoutine;
    
    public MonoBehaviour movementController;
    
    public static bool TrainStopUIActive { get; private set; }
    
    private bool inZone = false;
    
    void Start(){
        warningsUI.SetActive(false);
        trainStopUI.SetActive(false);
        worldInstuctionsUI.SetActive(false);
        TrainStopUIActive = false;
    }

    void OnTriggerEnter(Collider other){

        if (other.CompareTag("Player")){
            inZone = true;
            warningsUI.SetActive(true);
            worldInstuctionsUI.SetActive(true);
            
            var cg = worldInstuctionsUI.GetComponent<CanvasGroup>();
            if (cg != null) {
                
                cg.alpha = 1f;
            }
        }
     
    }

    void OnTriggerExit(Collider other) {
        
        if (other.CompareTag("Player")) {
            inZone = false;
            warningsUI.SetActive(false);
            trainStopUI.SetActive(false);
            FadeOutAndDisable(worldInstuctionsUI, worldFadeDelay, worldFadeDuration);
            movementController.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
    //similar to fade ui script but just for this worldInstuctionsUI
    private void FadeOutAndDisable(GameObject ui, float delay, float duration){
        // if mid-fade, stop it
        if (worldFadeRoutine != null){
            StopCoroutine(worldFadeRoutine);
        }
        worldFadeRoutine = StartCoroutine(FadeCoroutine(ui, delay, duration));
    }

    private IEnumerator FadeCoroutine(GameObject ui, float delay, float duration) {
    
        yield return new WaitForSeconds(delay);
        
        var cg = ui.GetComponent<CanvasGroup>();

        float startAlpha = cg.alpha;
        float elapsed = 0f;
        
        while (elapsed < duration){
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            yield return null;
        }

        cg.alpha = 0f;
        ui.SetActive(false);
    }
    
    void Update(){
        
        if (CameraManager.ScanModeActive || NotebookPages.NotebookOpen){
            return;
        }
           
        // open panel
        if (inZone && !trainStopUI.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            trainStopUI.SetActive(true);
            warningsUI.SetActive(false);
            worldInstuctionsUI.SetActive(false);
            movementController.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        
        }
        TrainStopUIActive = trainStopUI.activeSelf;
        
        if (!TrainStopUIActive) {
            Time.timeScale = 1f;
        }else{ 
            Time.timeScale = 0f;
        }
    }
    

}