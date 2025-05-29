using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipIntroManager : MonoBehaviour{
    
    public Camera introCamera;
    public Camera shipCamera;
    public Transform cameraTargetPosition;

    private float cameraHoldTime  = 1f;    
    private float cameraMoveTime  = 2f;    
    private float finalHoldTime   = 1f;   

    public Image pressAnyKeyText;
    public MonoBehaviour[] scriptsToDisable;
    public MonoBehaviour[] scriptsToEnableDuringIntro;
    
    private float  inputTimer   = 0f;  
    private float inputDelay = 8f;
    
    private bool introStarted = false;
    private float timer = 0f;
    private Vector3 startPos;
    private Quaternion startRot;
    
    public GameObject wasdUi;

    
    //delete the stuff for enabeling or disableing scripts. make bools for each cammera to enable states
    
    void Start(){
        
        //Time.timeScale = 0f;
        introCamera.enabled = true;
        shipCamera.enabled  = false;
        pressAnyKeyText.gameObject.SetActive(false);

        wasdUi.SetActive(false);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        foreach (var script in scriptsToDisable) {
            if (script != null) {
                script.enabled = false;
            }
        }

        foreach (var script in scriptsToEnableDuringIntro){
            if (script != null){
                script.enabled = true;
            }            
        }

    }

    void Update(){
        
        inputTimer += Time.unscaledDeltaTime;
        if (introStarted){
            timer += Time.unscaledDeltaTime;
        }
        
        if (!introStarted) {
            if (inputTimer >= inputDelay)
                pressAnyKeyText.gameObject.SetActive(true);
            else
                return;  
        }

        if (!introStarted && Input.anyKeyDown){
            
            pressAnyKeyText.gameObject.SetActive(false);
            introStarted = true;
            timer = 0f;

            startPos = introCamera.transform.position;
            startRot = introCamera.transform.rotation;
        }

        if (!introStarted){
            return;
        }

        timer += Time.unscaledDeltaTime;

        // 1 Hold 
        if (timer < cameraHoldTime)
            return;

        // 2 Move
        float moveElapsed = timer - cameraHoldTime;
        float t = Mathf.Clamp01(moveElapsed / cameraMoveTime);
        float eased = Mathf.SmoothStep(0f, 1f, t);

        introCamera.transform.position = Vector3.Lerp(startPos, cameraTargetPosition.position, eased);
        introCamera.transform.rotation = Quaternion.Slerp(startRot, cameraTargetPosition.rotation, eased);

        // 3 Final hold then switch
        if (timer < cameraHoldTime + cameraMoveTime + finalHoldTime) {
            return;
        }
        
        shipCamera.enabled  = true;
        introCamera.enabled = false;

        foreach (var script in scriptsToDisable){
            if (script != null) {
                script.enabled = true;
            }
        }

        Time.timeScale = 1f;
        this.enabled = false;
    }
}
