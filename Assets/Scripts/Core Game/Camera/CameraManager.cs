using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour{
    [SerializeField] private GameObject firstPersonRoot;
    [SerializeField] private GameObject fpRig;
    [SerializeField] private GameObject thirdPersonRoot;

    [SerializeField] private Canvas firstPersonUI;
    [SerializeField] private Canvas thirdPersonUI;

    private KeyCode switchKey = KeyCode.Q;
    [SerializeField] private GameObject scannerUIRoot;

    private MonoBehaviour[] firstPersonScripts;
    private MonoBehaviour[] thirdPersonScripts;

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

        if (scannerUIRoot != null){}
            scannerUIRoot.SetActive(firstPersonActive);
    }


    public bool IsFirstPersonActive(){
        return isFirstPersonActive;
    }

}