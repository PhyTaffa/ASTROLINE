using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipIntroManager : MonoBehaviour
{

    public Camera introCamera;
    public Camera shipCamera;
    public Transform cameraTargetPosition;
    public float cameraMoveTime = 2f;

  
    public TextMeshProUGUI  pressAnyKeyText;


    public MonoBehaviour[] scriptsToDisable;
    public MonoBehaviour[] scriptsToEnableDuringIntro;

    private bool introStarted = false;
    private float timer = 0f;

    void Start()
    {
        Time.timeScale = 0f;
        introCamera.enabled = true;
        shipCamera.enabled = false;
        pressAnyKeyText.gameObject.SetActive(true);

        foreach (var script in scriptsToDisable)
        {
            if (script != null) script.enabled = false;
        }

        foreach (var script in scriptsToEnableDuringIntro)
        {
            if (script != null) script.enabled = true;
        }
    }

    void Update()
    {
        if (!introStarted && Input.anyKeyDown)
        {
            pressAnyKeyText.gameObject.SetActive(false);
            introStarted = true;
        }

        if (introStarted)
        {
            timer += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(timer / cameraMoveTime);

            // Smooth camera move over time
            introCamera.transform.position = Vector3.Lerp(
                introCamera.transform.position,
                cameraTargetPosition.position,
                progress
            );

            introCamera.transform.rotation = Quaternion.Slerp(
                introCamera.transform.rotation,
                cameraTargetPosition.rotation,
                progress
            );

            if (timer >= cameraMoveTime)
            {
                // Switch cameras
                shipCamera.enabled = true;
                introCamera.enabled = false;

                // Enable gameplay scripts
                foreach (var script in scriptsToDisable)
                {
                    if (script != null) script.enabled = true;
                }

                // Resume time
                Time.timeScale = 1f;

                // Disable self
                this.enabled = false;
            }
        }
    }
}
