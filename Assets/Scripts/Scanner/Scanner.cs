using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [Header("Scan Settings")]
    private Transform rayOrigin;
    public float scanRange = 5f;
    public LayerMask scannableLayer;
    public float scanTime = 2f;
    public Material highlightMaterial;

    [Header("Notebook and UI")]
    public NotebookManager notebookManager;
    public ScannerUIManager scannerUIManager;

    private float scanProgress = 0f;
    private bool isScanning = false;
    private Transform currentTarget;

    private Material originalMaterial;
    private Renderer targetRenderer;

    private Queue<GameObject> GOBuffer = new Queue<GameObject>();
    [SerializeField] private int maxGOBufferSize = 2;

    private CameraManager cameraManager;
    public Camera playerCamera; 

    private void Start()
    {
        GOBuffer.Enqueue(null);
        GOBuffer.Enqueue(null);

        cameraManager = FindObjectOfType<CameraManager>();
        rayOrigin = this.transform; // Scanner is already in the right place!

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
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
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * scanRange, Color.red);

        if (Physics.Raycast(ray, out hit, scanRange, scannableLayer))
        {
            if (currentTarget != hit.transform)
            {
                ResetHighlight();
                currentTarget = hit.transform;
                scanProgress = 0f;

                targetRenderer = currentTarget.GetComponent<Renderer>();
                if (targetRenderer != null)
                {
                    originalMaterial = targetRenderer.material;
                    targetRenderer.material = highlightMaterial;
                }
            }

            isScanning = true;
            scanProgress += Time.deltaTime;

            // ⭐ Update UI Progress here!
            if (scannerUIManager != null)
            {
                scannerUIManager.UpdateScanProgress(scanProgress / scanTime);
            }

            if (scanProgress >= scanTime && !GOBuffer.Contains(hit.transform.gameObject))
            {
                BufferScannedObject(hit.transform.gameObject);
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
        if (currentTarget == null) return;

        ScannableObject scannable = currentTarget.GetComponent<ScannableObject>();
        if (scannable != null && scannable.scanData != null)
        {
            notebookManager.AddEntry(scannable.scanData);

            if (scannerUIManager != null)
            {
                scannerUIManager.AddScan(scannable.scanData);
                scannerUIManager.UpdateScanProgress(0f); // Reset bar after success
            }

            Debug.Log("Scan completed: " + scannable.scanData.objectName);
        }
        ResetScan();
    }

    void ResetScan()
    {
        if (isScanning && scannerUIManager != null)
        {
            scannerUIManager.UpdateScanProgress(0f); // Clear bar if interrupted
        }

        isScanning = false;
        scanProgress = 0f;
        ResetHighlight();
    }

    void ResetHighlight()
    {
        if (targetRenderer != null && originalMaterial != null)
        {
            targetRenderer.material = originalMaterial;
            targetRenderer = null;
            originalMaterial = null;
        }
    }

    private void BufferScannedObject(GameObject scannedObject)
    {
        if (GOBuffer.Count >= maxGOBufferSize)
        {
            GOBuffer.Dequeue();
        }
        GOBuffer.Enqueue(scannedObject);
    }
}