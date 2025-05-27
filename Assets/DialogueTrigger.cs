using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string tagToCheck = "Player";
    private bool hasBeenTriggeredAlready = false;

    [SerializeField] private string textToDisplay = "Default dialogue text";
    [SerializeField] private float displayDuration = 3f;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggeredAlready && other.CompareTag(tagToCheck))
        {
            hasBeenTriggeredAlready = true;
            ShowDialogue();
        }
    }

    private void ShowDialogue()
    {
        dialogueText.text = textToDisplay;
        dialoguePanel.SetActive(true);
        StartCoroutine(TimedWindow(displayDuration));
    }

    private IEnumerator TimedWindow(float timer)
    {
        yield return new WaitForSeconds(timer);
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }
}

