using UnityEngine;

public class SunLightLookAtPlanet : MonoBehaviour
{
    public Transform planetTransform;

    void Update()
    {
        Vector3 dir = planetTransform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}

