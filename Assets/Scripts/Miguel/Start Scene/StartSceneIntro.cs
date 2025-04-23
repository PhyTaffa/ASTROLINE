using UnityEngine;
using UnityEngine.UI;

public class StartSceneIntro : MonoBehaviour
{
    private float fadeDuration = 5f;

    private Image fadeImage;
    private float timer = 0f;
    private bool fading = true;

    void Start()
    {
        fadeImage = GetComponent<Image>();
        Color black = fadeImage.color;
        black.a = 1f;
        fadeImage.color = black;
    }

    void Update()
    {
        if (fading)
        {
            timer += Time.deltaTime;
            float smoothTime = Mathf.SmoothStep(0f, 1f, timer / fadeDuration);
            float alpha = Mathf.Lerp(1f, 0f, smoothTime);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);

            if (timer >= fadeDuration)
            {
                fadeImage.color = new Color(0f, 0f, 0f, 0f);
                fading = false;
            }
        }
    }
}