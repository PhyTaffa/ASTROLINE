using Cinemachine;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float topClamp = -30f;
    public float bottomClamp = 30f;

    private float yaw = 0f; // left/right
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

        // --- Yaw (rotate player around their up axis)
        playerTransform.Rotate(playerTransform.up, mouseX, Space.World);

        // --- Pitch: Use Mathf.Clamp to keep pitch within bounds
        pitch -= mouseY; // Apply mouseY to pitch
        pitch = Mathf.Clamp(pitch, topClamp, bottomClamp); // Clamp pitch to the specified range


        //apply the pitch to the camera pivot (only the pitch, no yaw)
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}