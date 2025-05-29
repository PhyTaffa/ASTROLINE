using UnityEngine;

public class ConstantCameraShake : MonoBehaviour{
    
    private float shake = 0.01f;

    private Vector3 originalLocalPos;

    void Awake(){

        originalLocalPos = transform.localPosition;
    }

    void LateUpdate(){
        // random jitter
        Vector3 jitter = Random.insideUnitSphere * shake;
        jitter.z = 0f; 
        // depth unchanged

        transform.localPosition = originalLocalPos + jitter;
    }
}