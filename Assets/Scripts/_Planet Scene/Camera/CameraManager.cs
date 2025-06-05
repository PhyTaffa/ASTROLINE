using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour{
    [Header("1st Person Cm")]
    [SerializeField] private GameObject firstPersonRoot;
    [SerializeField] private GameObject fpRig;
    
    [Header("3rd Person Cam")]
    [SerializeField] private GameObject thirdPersonRoot;

    private KeyCode switchKey = KeyCode.Q;
    
    
    
    [Header("UIs")]
    [SerializeField] private GameObject scannerUIRoot;
    [SerializeField] private GameObject tpUI;
    
    private MonoBehaviour[] firstPersonScripts;
    private MonoBehaviour[] thirdPersonScripts;

    [Header("Debug var")]
    [SerializeField] private bool isFirstPersonActive = true;

    private CinemachineVirtualCamera firstPersonCam;
    private CinemachineVirtualCamera thirdPersonCam;
    
    
    public static bool ScanModeActive { get; private set; }
    
    void Start(){
        firstPersonScripts = firstPersonRoot.GetComponentsInChildren<MonoBehaviour>(true);
        thirdPersonScripts = thirdPersonRoot.GetComponentsInChildren<MonoBehaviour>(true);

        firstPersonCam = firstPersonRoot.GetComponentInChildren<CinemachineVirtualCamera>(true);
        thirdPersonCam = thirdPersonRoot.GetComponentInChildren<CinemachineVirtualCamera>(true);
        
        ScanModeActive = scannerUIRoot != null && scannerUIRoot.activeSelf;
        SetCameraState(isFirstPersonActive);
        
        // if (isFirstPersonActive) {
        //     scannerCamEffect.SetActive(false);
        //     planetCamEffect.SetActive(true);
        // }else {
        //     scannerCamEffect.SetActive(true);
        //     planetCamEffect.SetActive(false);
        // }
    }

    private void Update(){
        
        ScanModeActive = scannerUIRoot.activeSelf;
        
        if (NotebookPages.NotebookOpen || TrainStopInteractor.TrainStopUIActive) {
            return;
        }
        
        if (Input.GetKeyDown(switchKey)){
            isFirstPersonActive = !isFirstPersonActive;
            
            fpRig.SetActive(isFirstPersonActive);
            
            SetCameraState(isFirstPersonActive);
        }
    }

    private void SetCameraState(bool firstPersonActive){
        
        firstPersonCam.Priority = firstPersonActive ? 10 : 0;
        thirdPersonCam.Priority = firstPersonActive ? 0 : 10;

        foreach (MonoBehaviour script in firstPersonScripts)
            script.enabled = firstPersonActive;

        foreach (MonoBehaviour script in thirdPersonScripts)
            script.enabled = !firstPersonActive;
        
        //scannerUIRoot.SetActive(firstPersonActive);
        //tpUI.SetActive(!firstPersonActive);
            
    }


    public bool IsFirstPersonActive(){
        return isFirstPersonActive;
    }

}