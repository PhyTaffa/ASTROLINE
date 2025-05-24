using UnityEngine;

public class AsteroidRotation : MonoBehaviour{

    private float minSpeed = 10f;
    private float maxSpeed = 1200f;

    private Vector3 spinAxis;
    private float spinSpeed;

    void Start(){
        spinAxis  = Random.onUnitSphere;
        spinSpeed = Random.Range(minSpeed, maxSpeed);
    }

    void Update(){
        transform.Rotate(spinAxis, spinSpeed * Time.deltaTime, Space.World);
    }
}