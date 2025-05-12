using UnityEngine;

public class ShipPlayerController : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float rotationSpeed = 360f;

    private Rigidbody rb;

    void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate(){
        
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // desired movement direction
        Vector3 dir = new Vector3(h, 0f, v).normalized;
        if (dir.sqrMagnitude < 0.01f){
            return; 
        }
        
        // Calculate the yaw-only rotation
        float targetYaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float newYaw = Mathf.MoveTowardsAngle(rb.rotation.eulerAngles.y, targetYaw, rotationSpeed * Time.fixedDeltaTime);
        Quaternion rot = Quaternion.Euler(0f, newYaw, 0f);
        
        rb.MoveRotation(rot);
        Vector3 nextPos = rb.position + dir * (moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(nextPos);
    }
}