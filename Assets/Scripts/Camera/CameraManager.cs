using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject firstPersonRoot;
    [SerializeField] private GameObject thirdPersonRoot;
    //[SerializeField] private MonoBehaviour[] firstPersonScripts;
    //[SerializeField] private MonoBehaviour[] thirdPersonScripts;
    [SerializeField] private KeyCode switchKey = KeyCode.V;
    [SerializeField] private GameObject scannerUIRoot;

    private MonoBehaviour[] firstPersonScripts;
    private MonoBehaviour[] thirdPersonScripts;

    [SerializeField] private bool isFirstPersonActive = true;

    private Camera firstPersonCamera = null;
    private Camera thirdPersonCamera = null;

    void Start()
    {
        firstPersonScripts = firstPersonRoot.GetComponentsInChildren<MonoBehaviour>(true);
        thirdPersonScripts = thirdPersonRoot.GetComponentsInChildren<MonoBehaviour>(true);
        
        firstPersonCamera = firstPersonRoot.GetComponent<Camera>();
        thirdPersonCamera = thirdPersonRoot.GetComponent<Camera>();

        SetCameraState(isFirstPersonActive);
    }
    private void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            //reset the 3rd camera to the player's back
            isFirstPersonActive = !isFirstPersonActive;
            SetCameraState(isFirstPersonActive);
        }
    }

    private void SetCameraState(bool firstPersonActive)
    {
        firstPersonCamera.enabled = firstPersonActive;
        thirdPersonCamera.enabled = !firstPersonActive;

        foreach (MonoBehaviour script in firstPersonScripts)
            script.enabled = firstPersonActive;

        foreach (MonoBehaviour script in thirdPersonScripts)
            script.enabled = !firstPersonActive;

        if (scannerUIRoot != null)
            scannerUIRoot.SetActive(firstPersonActive);
    }
}
