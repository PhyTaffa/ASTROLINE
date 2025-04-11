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

    private List<ScanData> scannedEntries = new List<ScanData>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            notebookUI.SetActive(!notebookUI.activeSelf);
            if (notebookUI.activeSelf && scannedEntries.Count > 0)
            {
                ShowEntry(0); // Show the first entry
            }
        }
    }


    public void AddEntry(ScanData data)
    {
        if (!scannedEntries.Contains(data))
        {
            scannedEntries.Add(data);
            Debug.Log("Added new entry to notebook: " + data.objectName);
        }
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
