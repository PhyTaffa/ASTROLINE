using UnityEngine;

public class RotatePreview : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}