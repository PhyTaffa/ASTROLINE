using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotebookManager : MonoBehaviour
{
    public GameObject notebookUI; // Panel to enable/disable
    public Image displayImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI habitatText;
    public GameObject player; // Reference to player object (for disabling movement script)

    private List<ScanData> scannedEntries = new List<ScanData>();
    private bool isNotebookOpen = false;

    void Start()
    {
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleNotebook();
        }
    }

    void ToggleNotebook()
    {
        isNotebookOpen = !isNotebookOpen;
        notebookUI.SetActive(isNotebookOpen);

        if (isNotebookOpen && scannedEntries.Count > 0)
        {
            ShowEntry(0); // Show the first entry
        }

        // Pause game and disable player movement
        if (isNotebookOpen)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Disable your movement script (adjust the script name if needed)
            //player.GetComponent<PlayerMovement>().enabled = false;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //player.GetComponent<PlayerMovement>().enabled = true;
        }
    }

    public void AddEntry(ScanData data)
    {

    }


    public void ShowEntry(int index)
    {
        if (index < 0 || index >= scannedEntries.Count) return;

        ScanData data = scannedEntries[index];
        nameText.text = data.objectName;
        descText.text = data.description;
        locationText.text = "Location: " + data.location;
        habitatText.text = "Habitat: " + data.habitat;
        displayImage.sprite = data.objectIcon;
    }
}
