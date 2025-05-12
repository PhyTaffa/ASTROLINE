using UnityEngine;
using UnityEngine.UI;

public class StartSceneIntro : MonoBehaviour {
    
    public Image fadeImage;
    public Image logo;
    public Image whiteBackground;
    
    private float fadeOutDuration = 4f;
    private float holdTime = 1f;
    private float fadeInDuration = 2f;
    private float finalHoldTime = 1f;
    private float finalFadeOutDuration = 4f;
  
    
    private float timer = 0f;
    private enum State { FadeOut, Hold, FadeIn, FinalHold, FinalFadeOut, Done }
    private State currentState = State.FadeOut;

    void Start(){
        
        // Start fully black
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update(){
        
        timer += Time.deltaTime;

        switch (currentState){
            
            case State.FadeOut:{
                
                float smoothTime = Mathf.SmoothStep(0f, 1f, timer / fadeOutDuration);
                float alpha = Mathf.Lerp(1f, 0f, smoothTime);
                SetAlpha(alpha);

                if (timer >= fadeOutDuration) {
                    
                    timer = 0f;
                    currentState = State.Hold;
                }
                break;
            }
            
            case State.Hold:{
                
                if (timer >= holdTime) {
                    
                    timer = 0f;
                    currentState = State.FadeIn;
                }
                break;
            }
            
            case State.FadeIn: {
                
                float smoothTime = Mathf.SmoothStep(0f, 1f, timer / fadeInDuration);
                float alpha = Mathf.Lerp(0f, 1f, smoothTime);
                SetAlpha(alpha);

                if (timer >= fadeInDuration){
                    
                    currentState = State.FinalHold;
                }
                break;
            }
            
            case State.FinalHold:{
                
                if (timer >= finalHoldTime){
                    logo.gameObject.SetActive(false);
                    whiteBackground.gameObject.SetActive(false);
                    timer = 0f;
                    currentState = State.FinalFadeOut;
                }
                break;
            }

            case State.FinalFadeOut:{
                
                float smoothTime = Mathf.SmoothStep(0f, 1f, timer / finalFadeOutDuration);
                float alpha = Mathf.Lerp(1f, 0f, smoothTime);
                SetAlpha(alpha);

                if (timer >= finalFadeOutDuration){
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    currentState = State.Done;
                }
                break;
            }
        }
    }

    void SetAlpha(float a)
    {
        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }
}