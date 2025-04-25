using Cinemachine;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float topClamp = -30f;
    public float bottomClamp = 30f;

    private float yaw = 0f;   // left/right
    private float pitch = 0f; // up/down

    private Transform playerTransform;
    private Quaternion startingPlayerRotation;
    private Quaternion startingCameraLocalRotation;

    public Transform cameraPivot;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        startingPlayerRotation = playerTransform.rotation;
        startingCameraLocalRotation = transform.localRotation;

        yaw = 0f;
        pitch = 0f;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        #region no clue

                // // Always use the player's current up vector for yaw
                // Quaternion yawRot = Quaternion.AngleAxis(yaw, playerTransform.up);
                // playerTransform.rotation = startingPlayerRotation * yawRot;
                //
                // // Use the rotated player's right axis for pitch
                // Vector3 playerRight = playerTransform.right;
                // Quaternion pitchRot = Quaternion.AngleAxis(pitch, playerRight);
                // transform.localRotation = startingCameraLocalRotation * pitchRot;

        #endregion

         // --- Yaw (rotate player around their up axis)
        playerTransform.Rotate(playerTransform.up, mouseX, Space.World);

        // --- Pitch: Use Mathf.Clamp to keep pitch within bounds
        pitch -= mouseY; // Apply mouseY to pitch
        pitch = Mathf.Clamp(pitch, topClamp, bottomClamp); // Clamp pitch to the specified range

    
        // Apply the pitch to the camera pivot (only the pitch, no yaw)
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        //transform.rotation = pitchRotation * playerTransform.rotation;
        
        Debug.DrawRay(playerTransform.position, playerTransform.up * 200f, Color.green); // up
        Debug.DrawRay(playerTransform.position, playerTransform.right * 200f, Color.red); // right
        Debug.DrawRay(transform.position, transform.forward * 200f, Color.blue); // camera look
        
    }

    public void ResetOrientationFromCurrent()
    {
        startingPlayerRotation = playerTransform.rotation;
        startingCameraLocalRotation = transform.localRotation;
        yaw = 0f;
        pitch = 0f;
    }
}