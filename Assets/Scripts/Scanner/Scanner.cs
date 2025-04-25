using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [Header("Scan Settings")]
    public Camera playerCamera;
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

    private void Start()
    {
        GOBuffer.Enqueue(null);
        GOBuffer.Enqueue(null);
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
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
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
            }

            Debug.Log("Scan completed: " + scannable.scanData.objectName);
        }
    }

    void ResetScan()
    {
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
