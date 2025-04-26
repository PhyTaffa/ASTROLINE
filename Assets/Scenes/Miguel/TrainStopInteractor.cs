using UnityEngine;

public class TrainStopInteractor : MonoBehaviour
{
    [Tooltip("The UI panel with Cancel/Confirm buttons")]
    public GameObject trainStopUI;

    [Tooltip("Your player‐movement script, so we can disable it while the UI is up")]
    public MonoBehaviour movementController;

    private bool inZone = false;

    void Start()
    {
        trainStopUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            inZone = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
            trainStopUI.SetActive(false);
            movementController.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        // open panel
        if (inZone && !trainStopUI.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            trainStopUI.SetActive(true);
            movementController.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}