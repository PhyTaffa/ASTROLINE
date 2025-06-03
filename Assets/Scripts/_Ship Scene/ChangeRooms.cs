using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeRooms : MonoBehaviour{
    
    public Transform teleportSpot2;
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    private bool playerInside = false;
    private bool isTransitioning = false;
    
    public Transform playerTransform;
    public MonoBehaviour movementScript;



    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player") && !isTransitioning){
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            playerInside = false;
        }
    }

    private void Update(){
      
        if (playerInside && !isTransitioning){
            if (Input.GetKeyDown(KeyCode.E)){
                StartCoroutine(FadeAndTeleport());
            }
        }
    }

    private IEnumerator FadeAndTeleport(){
        if (teleportSpot2 == null || fadeImage == null || playerTransform == null){
            yield break;
        }
        movementScript.enabled = false;
        isTransitioning = true;
        
        var rb = playerTransform.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        
        yield return StartCoroutine(Fade(0f, 1f));
        
        playerTransform.position = teleportSpot2.position;
        playerTransform.rotation = teleportSpot2.rotation;
        
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        yield return StartCoroutine(Fade(1f, 0f));
        movementScript.enabled = true;
        isTransitioning = false;
    }


    private IEnumerator Fade(float startAlpha, float endAlpha){
        
   
        float elapsed = 0f;
        Color c = fadeImage.color;
        c.a = startAlpha;
        fadeImage.color = c;

        while (elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            c.a = newAlpha;
            fadeImage.color = c;
            yield return null;
        }
        
        c.a = endAlpha;
        fadeImage.color = c;
    }
}
