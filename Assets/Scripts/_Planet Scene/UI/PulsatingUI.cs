using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PulsatingUI : MonoBehaviour {

    //needs CanvasGroup
    
    private float fadeDuration = 1f;
    private float holdAtMax = 0.25f;
    private float holdAtMin = 0.25f;
    private float minAlpha = 0.2f;
    private float maxAlpha = 1f;           

    private CanvasGroup cg;
    private Coroutine   pulseRoutine;

    void Awake()
    {

        cg = GetComponent<CanvasGroup>();
        cg.alpha = maxAlpha;
    }

    void OnEnable(){
        
        StartPulse();
    }

    void OnDisable(){

        if (pulseRoutine != null){
            StopCoroutine(pulseRoutine);
        }
         
    }

    void StartPulse(){

        if (pulseRoutine != null){
            StopCoroutine(pulseRoutine);
        }
         

        pulseRoutine = StartCoroutine(PulseLoop());
    }

    IEnumerator PulseLoop(){
        while (true) {
            
            // hold at max
            yield return new WaitForSeconds(holdAtMax);

            // fade max → min
            yield return Fade(maxAlpha, minAlpha);

            // hold at min
            yield return new WaitForSeconds(holdAtMin);

            // fade min → max
            yield return Fade(minAlpha, maxAlpha);
        }
    }

    IEnumerator Fade(float from, float to){
        float elapsed = 0f;

        while (elapsed < fadeDuration){
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        cg.alpha = to;
    }

}
