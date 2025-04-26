using UnityEngine;
using System.Collections;

public class ComputerUISlider : MonoBehaviour {
    
    
    public RectTransform panelRect;
    public GameObject  computerButton;
    public GameObject backgroundButton;
    
    private float slideDistance = 200f;
    private float slideTime = 0.5f;

    public bool isAtInitial = true;
    public bool isAtDown = false;
    public bool isMovingPublic { get { return isMoving; } }

    Vector2 initialPos;
    bool isMoving = false;

    void Awake() {
        initialPos = panelRect.anchoredPosition;
        isAtInitial = true;
        isAtDown = false;

        if (computerButton != null){
            computerButton.SetActive(false);
        }


        if (backgroundButton != null){
            backgroundButton.SetActive(isAtInitial);    
        }
       
    }

    public void SlideDown() {

        if (!isMoving && isAtInitial) {
            StartCoroutine(Slide(true));
        }
           
    }

    public void SlideUp(){

        if (!isMoving && isAtDown){
            StartCoroutine(Slide(false));
        }
            
    }

    IEnumerator Slide(bool down) {
        isMoving = true;
        
        if (computerButton != null){
            computerButton.SetActive(false);
        }

        if (backgroundButton != null){
            backgroundButton.SetActive(false);
        }

        Vector2 start = panelRect.anchoredPosition;
        Vector2 end = down ? initialPos + Vector2.down * slideDistance : initialPos;

        float elapsed = 0f;
        while (elapsed < slideTime) {
            elapsed += Time.deltaTime;
            float p = Mathf.Clamp01(elapsed / slideTime);
            panelRect.anchoredPosition = Vector2.Lerp(start, end, p);
            yield return null;
        }

        panelRect.anchoredPosition = end;
        isMoving = false;
        isAtDown = down;
        isAtInitial = !down;

        // only show when fully down
        if (computerButton != null) {
            computerButton.SetActive(down);
        }

        if (backgroundButton != null){
            backgroundButton.SetActive(isAtInitial);
        } 
    }
}