using UnityEngine;

public class SunBillboard : MonoBehaviour
{
    public Transform cameraTransform;
    public float sunDistance = 1000f;

    void LateUpdate()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        transform.position = cameraTransform.position + cameraTransform.forward * sunDistance;
        transform.LookAt(cameraTransform.position);
    }
}
