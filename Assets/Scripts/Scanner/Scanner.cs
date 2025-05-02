using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour{
    

    public float scanRange = 5f;
    public LayerMask scannableLayer;
    public float scanTime = 2f;
    public Material highlightMaterial;
    
    private const string HAS_SCANNED_ANY = "HAS_SCANNED_ANY";

    // Flora
    private const string HAS_SCANNED_VIOLET_SPIKEWEED = "HAS_SCANNED_VIOLET_SPIKEWEED";
    private const string HAS_SCANNED_TOWERING_UNRAVELER = "HAS_SCANNED_TOWERING_UNRAVELER";
    private const string HAS_SCANNED_CRAB_COMMUNE = "HAS_SCANNED_CRAB_COMMUNE";
    private const string HAS_SCANNED_CONSTELLATED_GANGLION_TRAY = "HAS_SCANNED_CONSTELLATED_GANGLION_TRAY";
    private const string HAS_SCANNED_SPONGE_STONE = "HAS_SCANNED_SPONGE_STONE";
    private const string HAS_SCANNED_CLUSTERED_SLIME_MOLD = "HAS_SCANNED_CLUSTERED_SLIME_MOLD";

    // Fauna
    private const string HAS_SCANNED_AXOLOWYRM = "HAS_SCANNED_AXOLOWYRM";
    private const string HAS_SCANNED_BROODBACK_FROG_UNBOUND = "HAS_SCANNED_BROODBACK_FROG_UNBOUND";
    private const string HAS_SCANNED_BROODBACK_FROG_EGGBOUND = "HAS_SCANNED_BROODBACK_FROG_EGGBOUND";
    private const string HAS_SCANNED_BROODBELLY_FROG = "HAS_SCANNED_BROODBELLY_FROG";
    private const string HAS_SCANNED_GREATER_LEMON_SLUG = "HAS_SCANNED_GREATER_LEMON_SLUG";
    private const string HAS_SCANNED_WANDERING_SKY_JELLY = "HAS_SCANNED_WANDERING_SKY_JELLY";

    private Transform rayOrigin;
    private float scanProgress = 0f;
    private bool isScanning = false;
    private Transform currentTarget;
    private Material originalMaterial;
    private Renderer targetRenderer;

    private Queue<GameObject> GOBuffer = new Queue<GameObject>();
    [SerializeField] private int maxGOBufferSize = 2;

    void Start(){

        GOBuffer.Enqueue(null);
        GOBuffer.Enqueue(null);

        rayOrigin = transform;
    }

    void Update(){
        
        // cheat O to change all to false
        if (Input.GetKeyDown(KeyCode.O)){
            
            PlayerPrefs.SetInt(HAS_SCANNED_ANY, 0);

            // Flora
            PlayerPrefs.SetInt(HAS_SCANNED_VIOLET_SPIKEWEED, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_TOWERING_UNRAVELER, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_CRAB_COMMUNE, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_SPONGE_STONE, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_CLUSTERED_SLIME_MOLD, 0);

            // Fauna
            PlayerPrefs.SetInt(HAS_SCANNED_AXOLOWYRM, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_BROODBACK_FROG_UNBOUND, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_BROODBACK_FROG_EGGBOUND, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_BROODBELLY_FROG, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_GREATER_LEMON_SLUG, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_WANDERING_SKY_JELLY, 0);

            PlayerPrefs.Save();
            Debug.Log("All scan flags reset to FALSE");
        }
        
        if (Input.GetKeyDown(KeyCode.P)){
            
           
            PlayerPrefs.SetInt(HAS_SCANNED_AXOLOWYRM, 1);
            PlayerPrefs.Save();
        }
        
        if (Input.GetKey(KeyCode.E)){
            ScanForObjects();
        }else{
            ResetScan();
        }
           
    }

    void ScanForObjects(){
        
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, scanRange, scannableLayer)){
            
            if (currentTarget != hit.transform){
                
                ResetHighlight();
                currentTarget = hit.transform;
                scanProgress = 0f;
                targetRenderer = currentTarget.GetComponent<Renderer>();

                if (targetRenderer != null) {
                    originalMaterial = targetRenderer.material;
                    targetRenderer.material = highlightMaterial;
                }
            }

            isScanning = true;
            scanProgress += Time.deltaTime;

            if (scanProgress >= scanTime && !GOBuffer.Contains(hit.transform.gameObject)){
                
                GOBuffer.Dequeue();
                GOBuffer.Enqueue(hit.transform.gameObject);
                CompleteScan();
            }
            
        }else{
            ResetScan();
        }
    }

    void CompleteScan(){
    
        PlayerPrefs.SetInt(HAS_SCANNED_ANY, 1);
        
        var scannable = currentTarget.GetComponent<ScannableObject>();
        if (scannable != null && scannable.scanData != null){
            
            string id = scannable.scanData.objectName;

            switch (id){
                
                case "Violet Spikeweed":
                    PlayerPrefs.SetInt(HAS_SCANNED_VIOLET_SPIKEWEED, 1);
                    break;
                case "Towering Unraveler":
                    PlayerPrefs.SetInt(HAS_SCANNED_TOWERING_UNRAVELER, 1);
                    break;
                case "Crab Commune":
                    PlayerPrefs.SetInt(HAS_SCANNED_CRAB_COMMUNE, 1);
                    break;
                case "Constellated Ganglion Tray":
                    PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY, 1);
                    break;
                case "Spongestone":
                    PlayerPrefs.SetInt(HAS_SCANNED_SPONGE_STONE, 1);
                    break;
                case "Clustered Slime Mold":
                    PlayerPrefs.SetInt(HAS_SCANNED_CLUSTERED_SLIME_MOLD, 1);
                    break;
                case "Axolowyrm":
                    PlayerPrefs.SetInt(HAS_SCANNED_AXOLOWYRM, 1);
                    break;
                case "Broodback Frog (Unbound)":
                    PlayerPrefs.SetInt(HAS_SCANNED_BROODBACK_FROG_UNBOUND, 1);
                    break;
                case "Broodback Frog (Eggbound)":
                    PlayerPrefs.SetInt(HAS_SCANNED_BROODBACK_FROG_EGGBOUND, 1);
                    break;
                case "Broodbelly Frog":
                    PlayerPrefs.SetInt(HAS_SCANNED_BROODBELLY_FROG, 1);
                    break;
                case "Greater Lemon Slug":
                    PlayerPrefs.SetInt(HAS_SCANNED_GREATER_LEMON_SLUG, 1);
                    break;
                case "Wandering Sky Jelly":
                    PlayerPrefs.SetInt(HAS_SCANNED_WANDERING_SKY_JELLY, 1);
                    break;
            }

            PlayerPrefs.Save();
            Debug.Log($"Scan completed: {id}");
        }

        ResetScan();
    }

    void ResetScan() {

        if (isScanning) {
            Debug.Log("Scan interrupted/reset");
        }
        
        isScanning    = false;
        scanProgress  = 0f;
        currentTarget = null;
        ResetHighlight();
    }

    void ResetHighlight(){
        
        if (targetRenderer != null && originalMaterial != null){
            targetRenderer.material = originalMaterial;
            targetRenderer         = null;
            originalMaterial       = null;
        }
    }
}
