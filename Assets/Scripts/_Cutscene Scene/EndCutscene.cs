using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndCutscene : MonoBehaviour{
    

    [SerializeField] private VideoPlayer videoPlayer;   
    [SerializeField] private Image fadeOverlay;         
    [SerializeField] private float fadeDuration = 0.5f; 

    private void Start() {
    
        SetOverlayAlpha(0f);
        
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnDestroy(){
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp){
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad(){
        
        float elapsed = 0f;
        Color c = fadeOverlay.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            fadeOverlay.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeOverlay.color = c;


        SceneManager.LoadScene("ShipMenu");
    }

    private void SetOverlayAlpha(float alpha){
        Color c = fadeOverlay.color;
        c.a = alpha;
        fadeOverlay.color = c;
    }
}