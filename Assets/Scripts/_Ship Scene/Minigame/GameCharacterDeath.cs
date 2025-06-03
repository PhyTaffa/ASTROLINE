using Unity.VisualScripting;
using UnityEngine;

public class GameCharacterDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Avatar"))
        {
            GameState.IsRunning = false;
        }
        if (other.CompareTag("Goal"))
        {
            Destroy(other.gameObject);
        }
    }
}