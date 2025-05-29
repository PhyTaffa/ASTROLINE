using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using UnityEngine;
using TMPro;

public class Scanner : MonoBehaviour
{
    [SerializeField, Tooltip("Current scan range, auto-adjusted based on upgrades.")]
    private float scanRange;
    private bool isZoomed = false;
    [SerializeField] private Camera mainCamera;
    private Transform cameraTransform;
    [SerializeField] private Animator zoomAnimator;
    public LayerMask scannableLayer;
    private float scanTime = 2f;
    public Material highlightMaterial;

    [SerializeField] private GameObject alreadyScannedUI;
    [SerializeField] private GameObject newScanEntryUI; 
    
    [SerializeField] private GameObject focusCenterRed;
    [SerializeField] private GameObject noBatteryWarning;
    [SerializeField] private BatteryUI battery;



    public bool scanningEnabled { get; private set; }

    void EnterFirstPersonScanner()
    {
        // your existing code that sets up FPS camera…
        scanningEnabled = true;
    }

    void ExitFirstPersonScanner()
    {
        // your existing code that tears down FPS camera…
        scanningEnabled = false;
    }

    private const string HAS_SCANNED_ANY = "HAS_SCANNED_ANY";

    // Flora — Flowers
    private const string HAS_SCANNED_VIOLET_SPIKEWEED                               = "HAS_SCANNED_VIOLET_SPIKEWEED";
    private const string HAS_SCANNED_TOWERING_UNRAVELER_YOUNG                       = "HAS_SCANNED_TOWERING_UNRAVELER_YOUNG";
    private const string HAS_SCANNED_TOWERING_UNRAVELER_JUVENILE                    = "HAS_SCANNED_TOWERING_UNRAVELER_JUVENILE";
    private const string HAS_SCANNED_TOWERING_UNRAVELER_ADULT                       = "HAS_SCANNED_TOWERING_UNRAVELER_ADULT";
               
    // Flora — Funguse 
    private const string HAS_SCANNED_CLUSTERED_SLIME_MOLD                           = "HAS_SCANNED_CLUSTERED_SLIME_MOLD";
    private const string HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_YOUNG        = "HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_YOUNG";
    private const string HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_JUVENILE     = "HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_JUVENILE";
    private const string HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_ADULT        = "HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_ADULT";
    private const string HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_YOUNG          = "HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_YOUNG";
    private const string HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_JUVENILE       = "HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_JUVENILE";
    private const string HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_ADULT          = "HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_ADULT";
   
    // Flora — Coral   
    private const string HAS_SCANNED_CRAB_COMMUNE_DEVELOPING                        = "HAS_SCANNED_CRAB_COMMUNE_DEVELOPING";
    private const string HAS_SCANNED_CRAB_COMMUNE_FULLY_DEVELOPED                   = "HAS_SCANNED_CRAB_COMMUNE_FULLY_DEVELOPED";
    private const string HAS_SCANNED_SPONGE_STONE                                   = "HAS_SCANNED_SPONGE_STONE";
    private const string HAS_SCANNED_SPONGE_STONE_INVERTED                          = "HAS_SCANNED_SPONGE_STONE_INVERTED";
   
    // Faun
    private const string HAS_SCANNED_AXOLOWYRM                                      = "HAS_SCANNED_AXOLOWYRM";
    private const string HAS_SCANNED_BROODBACK_FROG_UNBOUND                         = "HAS_SCANNED_BROODBACK_FROG_UNBOUND";
    private const string HAS_SCANNED_BROODBACK_FROG_EGGBOUND                        = "HAS_SCANNED_BROODBACK_FROG_EGGBOUND";
    private const string HAS_SCANNED_BROODBELLY_FROG                                = "HAS_SCANNED_BROODBELLY_FROG";
    private const string HAS_SCANNED_GREATER_LEMON_SLUG                             = "HAS_SCANNED_GREATER_LEMON_SLUG";
    private const string HAS_SCANNED_WANDERING_SKY_JELLY                            = "HAS_SCANNED_WANDERING_SKY_JELLY";
    private const string HAS_SCANNED_SHOCKSHIELD_CRAB                               = "HAS_SCANNED_SHOCKSHIELD_CRAB";
    private const string HAS_SCANNED_HALF_HEADED_AVIAN                              = "HAS_SCANNED_HALF_HEADED_AVIAN";
    private const string HAS_SCANNED_TRAILING_LANDSTAR                              = "HAS_SCANNED_TRAILING_LANDSTAR";
    
    //raycastin
    private Transform rayOrigin;
    private float scanProgress = 0f;
    private bool isScanning = false;
    private Transform currentTarget;
    
    //shader outliener
    private Material originalMaterial;
    private Renderer targetRenderer;
    private OutlineController lastOutline; //new
    

    private Queue<GameObject> GOBuffer = new Queue<GameObject>();
    [SerializeField] private int maxGOBufferSize = 2;

    
    //zoom var
    private float currFOV = 60f;
    private float maxFOV = 60f;
    private float minFOV = 30f;
    private float sensitivity = 120f;
    private Transform child;
    private CinemachineVirtualCamera vCam;
    
    void Start(){

        GOBuffer.Enqueue(null);
        GOBuffer.Enqueue(null);

        cameraTransform = mainCamera.GetComponent<Transform>();
        
        rayOrigin = cameraTransform;
        
        scanRange = 5f;
        
        //virtual cam shid
        child = this.transform.GetChild(0);
        vCam = child.GetComponent<CinemachineVirtualCamera>();
    }

    void Update(){
        
        
        if (PlayerPrefs.GetInt("UpgradeBlueEnabled", 0) == 1){
            if (Input.mouseScrollDelta.y != 0) {
                isZoomed = !isZoomed;
                scanRange = isZoomed ? 10f : 5f;
            }
            
        }else{
            scanRange = 5f;
        }
        
        bool isAtMaxZoom = scanRange > 5f;
        zoomAnimator.SetBool("MaxZoom", isAtMaxZoom);
           

        // Sync camera FOV with scan range
        vCam.m_Lens.FieldOfView = isAtMaxZoom ? minFOV : maxFOV;

        
        // cheat O to change all to false and P to true
        if (Input.GetKeyDown(KeyCode.O)){
            
            PlayerPrefs.SetInt(HAS_SCANNED_ANY, 0);

            // Flora — Flowers
            PlayerPrefs.SetInt(HAS_SCANNED_VIOLET_SPIKEWEED, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_TOWERING_UNRAVELER_YOUNG, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_TOWERING_UNRAVELER_JUVENILE, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_TOWERING_UNRAVELER_ADULT, 0);

            // Flora — Funguses
            PlayerPrefs.SetInt(HAS_SCANNED_CLUSTERED_SLIME_MOLD, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_YOUNG, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_JUVENILE, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_ADULT, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_YOUNG, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_JUVENILE, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_ADULT, 0);

            // Flora — Corals
            PlayerPrefs.SetInt(HAS_SCANNED_CRAB_COMMUNE_DEVELOPING, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_CRAB_COMMUNE_FULLY_DEVELOPED, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_SPONGE_STONE, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_SPONGE_STONE_INVERTED, 0);

            // Fauna
            PlayerPrefs.SetInt(HAS_SCANNED_AXOLOWYRM, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_BROODBACK_FROG_UNBOUND, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_BROODBACK_FROG_EGGBOUND, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_BROODBELLY_FROG, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_GREATER_LEMON_SLUG, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_WANDERING_SKY_JELLY, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_SHOCKSHIELD_CRAB, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_HALF_HEADED_AVIAN, 0);
            PlayerPrefs.SetInt(HAS_SCANNED_TRAILING_LANDSTAR, 0);
            
            PlayerPrefs.Save();
            Debug.Log("All scan flags reset to FALSE");
        }
        
        if (Input.GetKeyDown(KeyCode.P)){
            
           
            PlayerPrefs.SetInt(HAS_SCANNED_ANY, 1);

            // Flora — Flowers
            PlayerPrefs.SetInt(HAS_SCANNED_VIOLET_SPIKEWEED, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_TOWERING_UNRAVELER_YOUNG, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_TOWERING_UNRAVELER_JUVENILE, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_TOWERING_UNRAVELER_ADULT, 1);

            // Flora — Funguses
            PlayerPrefs.SetInt(HAS_SCANNED_CLUSTERED_SLIME_MOLD, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_YOUNG, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_JUVENILE, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_ADULT, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_YOUNG, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_JUVENILE, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_ADULT, 1);

            // Flora — Corals
            PlayerPrefs.SetInt(HAS_SCANNED_CRAB_COMMUNE_DEVELOPING, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_CRAB_COMMUNE_FULLY_DEVELOPED, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_SPONGE_STONE, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_SPONGE_STONE_INVERTED, 1);

            // Fauna
            PlayerPrefs.SetInt(HAS_SCANNED_AXOLOWYRM, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_BROODBACK_FROG_UNBOUND, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_BROODBACK_FROG_EGGBOUND, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_BROODBELLY_FROG, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_GREATER_LEMON_SLUG, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_WANDERING_SKY_JELLY, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_SHOCKSHIELD_CRAB, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_HALF_HEADED_AVIAN, 1);
            PlayerPrefs.SetInt(HAS_SCANNED_TRAILING_LANDSTAR, 1);
            
            PlayerPrefs.Save();
            Debug.Log("All scan flags reset to TRUE");
        }

     
        //check waht's sacnnable
        HandleOutlineHighlight(); // new
      
        if (Input.GetKey(KeyCode.E)) {
            
            if (battery == null) {
                focusCenterRed.SetActive(false);
                ResetScan();
                return;
            }
    
            if (battery.GetBatteryLevelIndex() == 4) {

                if (noBatteryWarning != null && !noBatteryWarning.activeSelf) {
                    noBatteryWarning.SetActive(true); 
                }

                focusCenterRed.SetActive(false);
                ResetScan();
                return;
            }

            if (noBatteryWarning != null && noBatteryWarning.activeSelf) {
                noBatteryWarning.SetActive(false);
            }

            focusCenterRed.SetActive(true);
            
            ScanForObjects();

        } else {
            focusCenterRed.SetActive(false);
            ResetScan();
        }

        //If battery has any electricity the warning of no battery goes off imediatly
        if (battery.GetBatteryLevelIndex() != 4){
            noBatteryWarning.SetActive(false);
        }
           
    }

    private void Zoom()
    {
        float zoomDelta = Input.mouseScrollDelta.y;

        //float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        // if (zoomDelta != 0)
        // {
        //     if(currFOV == maxFOV)
        //         currFOV = minFOV;
        //     else
        //     {
        //         currFOV = maxFOV;
        //     }
        // }
        
        if (zoomDelta > 0)
        {
            currFOV = maxFOV;
        }
        else if (zoomDelta < 0)
        {
            currFOV = minFOV;
        }
         
        //currFOV += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        //currFOV = Mathf.Clamp(currFOV, minFOV, maxFOV);
        
        vCam.m_Lens.FieldOfView = currFOV;
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
            
            var scannable = currentTarget.GetComponent<ScannableObject>();
            if (scannable == null || scannable.scanData == null) {
                ResetScan();
                return;
            }
            
            string key;
            switch (scannable.scanData.objectName){
                case "Violet Spikeweed":                             key = HAS_SCANNED_VIOLET_SPIKEWEED; break;
                case "Towering Unraveler (Young)":                   key = HAS_SCANNED_TOWERING_UNRAVELER_YOUNG; break;
                case "Towering Unraveler (Juvenile)":                key = HAS_SCANNED_TOWERING_UNRAVELER_JUVENILE; break;
                case "Towering Unraveler (Adult)":                   key = HAS_SCANNED_TOWERING_UNRAVELER_ADULT; break;
                case "Clustered Slime Mold":                         key = HAS_SCANNED_CLUSTERED_SLIME_MOLD; break;
                case "Constellated Ganglion Tray (Female, Young)":   key = HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_YOUNG; break;
                case "Constellated Ganglion Tray (Female, Juvenile)":key = HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_JUVENILE; break;
                case "Constellated Ganglion Tray (Female, Adult)":   key = HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_ADULT; break;
                case "Constellated Ganglion Tray (Male, Young)":     key = HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_YOUNG; break;
                case "Constellated Ganglion Tray (Male, Juvenile)":  key = HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_JUVENILE; break;
                case "Constellated Ganglion Tray (Male, Adult)":     key = HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_ADULT; break;
                case "Crab Commune (Developing)":                    key = HAS_SCANNED_CRAB_COMMUNE_DEVELOPING; break;
                case "Crab Commune (Fully-Developed)":               key = HAS_SCANNED_CRAB_COMMUNE_FULLY_DEVELOPED; break;
                case "Spongestone":                                  key = HAS_SCANNED_SPONGE_STONE; break;
                case "Spongestone (Inverted)":                       key = HAS_SCANNED_SPONGE_STONE_INVERTED; break;
                case "Axolowyrm":                                    key = HAS_SCANNED_AXOLOWYRM; break;
                case "Broodback Frog (Unbound)":                     key = HAS_SCANNED_BROODBACK_FROG_UNBOUND; break;
                case "Broodback Frog (Eggbound)":                    key = HAS_SCANNED_BROODBACK_FROG_EGGBOUND; break;
                case "Broodbelly Frog":                              key = HAS_SCANNED_BROODBELLY_FROG; break;
                case "Greater Lemon Slug":                           key = HAS_SCANNED_GREATER_LEMON_SLUG; break;
                case "Wandering Sky Jelly":                          key = HAS_SCANNED_WANDERING_SKY_JELLY; break;
                case "Shockshield Crab":                             key = HAS_SCANNED_SHOCKSHIELD_CRAB; break;
                case "Half-Headed Avian":                            key = HAS_SCANNED_HALF_HEADED_AVIAN; break;
                case "Trailing Landstar":                            key = HAS_SCANNED_TRAILING_LANDSTAR; break;
                default: key = scannable.scanData.objectName; break;
            }

            bool hasEverScanned = PlayerPrefs.GetInt(key, 0) == 1;
            bool inBuffer = GOBuffer.Contains(currentTarget.gameObject);

            if (scanProgress >= scanTime){
                if (hasEverScanned || inBuffer){
                    ShowAlreadyScanned();
                } else {
                    GOBuffer.Dequeue();
                    GOBuffer.Enqueue(currentTarget.gameObject);
                    ShowNewScanEntry();
                    CompleteScan();
                }
            }
        }
        else {
            ResetScan();
        }
    }

    
    void ShowAlreadyScanned() {
        alreadyScannedUI.SetActive(true);
        ResetScan();
    }
    void ShowNewScanEntry(){
        newScanEntryUI.SetActive(true);
    }
    
    void CompleteScan(){
    
        PlayerPrefs.SetInt(HAS_SCANNED_ANY, 1);
        
        var scannable = currentTarget.GetComponent<ScannableObject>();
        if (scannable != null && scannable.scanData != null){
            
            string id = scannable.scanData.objectName;

            switch (id){
                
                // Flowers
                case "Violet Spikeweed":
                    PlayerPrefs.SetInt(HAS_SCANNED_VIOLET_SPIKEWEED, 1);
                    break;
                case "Towering Unraveler (Young)":
                    PlayerPrefs.SetInt(HAS_SCANNED_TOWERING_UNRAVELER_YOUNG, 1);
                    break;
                case "Towering Unraveler (Juvenile)":
                    PlayerPrefs.SetInt(HAS_SCANNED_TOWERING_UNRAVELER_JUVENILE, 1);
                    break;
                case "Towering Unraveler (Adult)":
                    PlayerPrefs.SetInt(HAS_SCANNED_TOWERING_UNRAVELER_ADULT, 1);
                    break;

                // Funguses
                case "Clustered Slime Mold":
                    PlayerPrefs.SetInt(HAS_SCANNED_CLUSTERED_SLIME_MOLD, 1);
                    break;
                case "Constellated Ganglion Tray (Female, Young)":
                    PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_YOUNG, 1);
                    break;
                case "Constellated Ganglion Tray (Female, Juvenile)":
                    PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_JUVENILE, 1);
                    break;
                case "Constellated Ganglion Tray (Female, Adult)":
                    PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_FEMALE_ADULT, 1);
                    break;
                case "Constellated Ganglion Tray (Male, Young)":
                    PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_YOUNG, 1);
                    break;
                case "Constellated Ganglion Tray (Male, Juvenile)":
                    PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_JUVENILE, 1);
                    break;
                case "Constellated Ganglion Tray (Male, Adult)":
                    PlayerPrefs.SetInt(HAS_SCANNED_CONSTELLATED_GANGLION_TRAY_MALE_ADULT, 1);
                    break;

                // Corals
                case "Crab Commune (Developing)":
                    PlayerPrefs.SetInt(HAS_SCANNED_CRAB_COMMUNE_DEVELOPING, 1);
                    break;
                case "Crab Commune (Fully-Developed)":
                    PlayerPrefs.SetInt(HAS_SCANNED_CRAB_COMMUNE_FULLY_DEVELOPED, 1);
                    break;
                case "Spongestone":
                    PlayerPrefs.SetInt(HAS_SCANNED_SPONGE_STONE, 1);
                    break;
                case "Spongestone (Inverted)":
                    PlayerPrefs.SetInt(HAS_SCANNED_SPONGE_STONE_INVERTED, 1);
                    break;

                // Fauna 
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
                case "Shockshield Crab":
                    PlayerPrefs.SetInt(HAS_SCANNED_SHOCKSHIELD_CRAB, 1);
                    break;
                case "Half-Headed_Avian":
                    PlayerPrefs.SetInt(HAS_SCANNED_HALF_HEADED_AVIAN, 1);
                    break;
                case "Trailing Landstar":
                    PlayerPrefs.SetInt(HAS_SCANNED_TRAILING_LANDSTAR, 1);
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
    
    private void HandleOutlineHighlight()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, scanRange * 10f))
        {
            var outline = hit.transform.GetComponent<OutlineController>();
            

            if (outline != null && outline != lastOutline)
            {
                ClearLastOutline();
                int layer = hit.transform.gameObject.layer;

                if (layer == LayerMask.NameToLayer("Scannable"))
                {
                    outline.ShowOutline(Color.green);
                    //scannerHintText.text = "Scannable";
                }
                else
                {
                    outline.ShowOutline(Color.red);
                    //scannerHintText.text = "Non-Scannable";
                }

                lastOutline = outline;
            }
        }
        else
        {
            ClearLastOutline();
            //scannerHintText.text = "";
        }
    }

    private void ClearLastOutline()
    {
        if (lastOutline != null)
        {
            lastOutline.HideOutline();
            lastOutline = null;
        }
    }

    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(cameraTransform.position, cameraTransform.position + cameraTransform.forward * scanRange);
        Gizmos.DrawWireSphere(cameraTransform.position + cameraTransform.forward * scanRange, 0.2f);
    }

}
