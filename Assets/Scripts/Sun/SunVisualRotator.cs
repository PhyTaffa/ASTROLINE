using UnityEngine;

public class SunVisualRotator : MonoBehaviour
{
    public float rotationSpeed = 0.5f; // degrees per second (like real solar spin)

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

