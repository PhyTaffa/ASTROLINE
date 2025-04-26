using UnityEngine;

public class PlayerDetectionTrigger : MonoBehaviour
{
    private bool isPlayerInside = false;

    public bool IsPlayerInside => isPlayerInside; // Read-only access

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}