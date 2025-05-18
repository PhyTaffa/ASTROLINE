using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutUI : MonoBehaviour {
    public float fadeDelay = 2f;
    public float fadeDuration = 1f;

    private CanvasGroup canvasGroup;
    private Coroutine fadeRoutine;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (!canvasGroup) {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1f;
    }

    void OnEnable() {
        
        canvasGroup.alpha = 1f;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeOutAfterDelay());
    }

    IEnumerator FadeOutAfterDelay() {
        yield return new WaitForSeconds(fadeDelay);

        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < fadeDuration) {
            
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}