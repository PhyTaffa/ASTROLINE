using System.Collections;
using TMPro;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private string tagToCheck = "Player";
    [SerializeField] private string textToDisplay = "Default dialogue text";
    [SerializeField] private float displayDuration = 3f;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    private bool hasBeenTriggeredAlready = false;
    private CameraManager camManager;

    private void Start()
    {
        camManager = FindObjectOfType<CameraManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggeredAlready && other.CompareTag(tagToCheck))
        {
            // Only trigger if we're in third-person view
            if (camManager != null && camManager.IsFirstPersonActive()) return;

            hasBeenTriggeredAlready = true;
            ShowDialogue();
        }
    }

    private void ShowDialogue()
    {
        if (dialoguePanel != null && dialogueText != null)
        {
            dialogueText.text = textToDisplay;
            dialoguePanel.SetActive(true);
            StartCoroutine(TimedWindow(displayDuration));
        }
    }

    private IEnumerator TimedWindow(float timer)
    {
        yield return new WaitForSeconds(timer);
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }
}

