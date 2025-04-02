using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour
{
    public Camera playerCamera; // Assign the player's camera
    public float scanRange = 5f; // Max scan distance
    public LayerMask scannableLayer; // Assign "Scannable" layer in Inspector
    public Image scanProgressUI; // UI element for scan progress
    public float scanTime = 2f; // Time required to scan

    private float scanProgress = 0f;
    private bool isScanning = false;
    private Transform currentTarget;

    void Update()
    {
        if (Input.GetKey(KeyCode.E)) // Only scan when "E" is held down
        {
            ScanForObjects();
        }
        else
        {
            ResetScan();
        }
    }

    void ScanForObjects()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * scanRange, Color.red); // Visualize Raycast

        if (Physics.Raycast(ray, out hit, scanRange, scannableLayer))
        {
            Debug.Log("Raycast hit: " + hit.transform.name); // Debugging

            if (currentTarget != hit.transform)
            {
                currentTarget = hit.transform;
                scanProgress = 0f; // Reset progress if new target
            }

            isScanning = true;
            scanProgress += Time.deltaTime;

            if (scanProgressUI != null)
                scanProgressUI.fillAmount = scanProgress / scanTime;

            if (scanProgress >= scanTime)
            {
                CompleteScan();
            }
        }
        else
        {
            ResetScan();
        }
    }

    void CompleteScan()
    {
        Debug.Log("Scan Complete: " + currentTarget.name);
        ResetScan();
    }

    void ResetScan()
    {
        isScanning = false;
        scanProgress = 0f;
        if (scanProgressUI != null)
        {
            scanProgressUI.fillAmount = 0f;
        }
    }
}




