using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CameraZone : MonoBehaviour
{
    [Tooltip("The camera you want to force on when the player enters this trigger.")]
    public Camera cameraToActivate;

    private static Camera s_activeZoneCamera; 

    private void Reset()
    {
        // Force the Collider into trigger mode, so OnTriggerEnter will fire.
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // If there was already another zone camera active, turn it off:
        if (s_activeZoneCamera != null && s_activeZoneCamera != cameraToActivate)
        {
            s_activeZoneCamera.enabled = false;
        }

        // Turn on this camera:
        cameraToActivate.enabled = true;

        // Remember it as “the currently active zone camera.”
        s_activeZoneCamera = cameraToActivate;
    }

    // Note: We do NOT disable cameraToActivate in OnTriggerExit,
    // because you want it to remain active until you enter a different zone.
}