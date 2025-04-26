using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour {
    
    public static FadeManager Instance;      
    public Image fadeImage;                  
    private float fadeDuration = 2f;     
    private float initialHoldOnBlack = 3f;
    private float initialFadeDuration = 3f;
    
    void Awake(){
        
        if (Instance == null){
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            fadeImage.enabled = true;
            SetAlpha(1f);
            
        }else{
            
            Destroy(gameObject);
        }
    }

    IEnumerator  Start(){
        yield return new WaitForSecondsRealtime(initialHoldOnBlack);
        yield return FadeAlpha(0f, initialFadeDuration);
    }

  
    public void FadeToScene(string sceneName) {
        StartCoroutine(FadeOutIn(sceneName));
    }

    IEnumerator FadeOutIn(string sceneName) {
       
        yield return FadeAlpha(1f, fadeDuration);
        SceneManager.LoadScene(sceneName);
        yield return null;
        yield return FadeAlpha(0f, fadeDuration);
    }

    
    IEnumerator FadeAlpha(float target, float duration) {
        
        float start = fadeImage.color.a;
        float elapsed = 0f;

        while (elapsed < duration) {
            
            elapsed += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(start, target, elapsed / duration);
            SetAlpha(a);
            yield return null;
        }
        SetAlpha(target);
    }
    void SetAlpha(float a){
        
        var c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }
}