using UnityEditor.UIElements;
using UnityEngine;

public class PlayerDetectionTrigger : MonoBehaviour
{
    [SerializeField] private string givenTag;

    public bool IsPlayerInside { get; private set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(givenTag))
        {
            IsPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(givenTag))
        {
            IsPlayerInside = false;
        }
    }
}