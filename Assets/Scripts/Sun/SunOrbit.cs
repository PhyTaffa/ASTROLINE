using UnityEngine;

public class SunOrbit : MonoBehaviour
{
    public float orbitSpeed = 5f; // degrees per second

    void Update()
    {
        transform.Rotate(Vector3.right, orbitSpeed * Time.deltaTime);
    }
}

