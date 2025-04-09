using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour
{
    public Camera playerCamera; // Assign the player's camera
    public float scanRange = 5f; // Max scan distance
    public LayerMask scannableLayer; // Assign "Scannable" layer in Inspector
    public Image scanProgressUI; // UI element for scan progress
    public float scanTime = 2f; // Time required to scan
    public Material highlightMaterial;
    public NotebookManager notebookManager;

    private float scanProgress = 0f;
    private bool isScanning = false;
    private Transform currentTarget;

    [SerializeField] private GameObject currScanning = null;
    [SerializeField] private GameObject[] queueToArray;
    private Queue<GameObject> GOBuffer = new Queue<GameObject>();
    [SerializeField] private int maxGOBufferSize = 2;

    private Material originalMaterial;
    private Renderer targetRenderer;

    private void Start()
    {
        //buffering nulls to VOID  probwlms
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

        // visual Debug cuz filippio lZY
        if (Input.GetKey(KeyCode.Z))
        {
            Array.Clear(queueToArray, 0, queueToArray.Length);
            queueToArray = GOBuffer.ToArray();
            int i = 0;
            foreach (GameObject elementInArray in queueToArray)
            {
                Debug.Log($"element {i} is {elementInArray?.name ?? "null"}");
                i++;
            }
        }
    }

    void ScanForObjects()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * scanRange, Color.red);

        if (Physics.Raycast(ray, out hit, scanRange, scannableLayer))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);

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

            if (scanProgressUI != null)
                scanProgressUI.fillAmount = scanProgress / scanTime;

            if (scanProgress >= scanTime && !GOBuffer.Contains(hit.transform.gameObject))
            {
                BufferGOIstance(hit.transform.gameObject);
                CompleteScan();
            }
        }
        else
        {
            ResetScan();
        }
    }

    private void BufferGOIstance(GameObject GOToValidate)
    {
        if (GOBuffer.Count >= maxGOBufferSize)
        {
            GOBuffer.Dequeue();
        }
        GOBuffer.Enqueue(GOToValidate);
    }

    void CompleteScan()
    {
        ScannableObject scannable = currentTarget.GetComponent<ScannableObject>();
        if (scannable != null && scannable.scanData != null)
        {
            notebookManager.AddEntry(scannable.scanData);
        }
        Debug.Log("Adding to notebook: " + scannable.scanData.objectName);

    }


    void ResetScan()
    {
        isScanning = false;
        scanProgress = 0f;
        if (scanProgressUI != null)
        {
            scanProgressUI.fillAmount = 0f;
        }
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
}





