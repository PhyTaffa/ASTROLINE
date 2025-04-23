using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCameraManager : MonoBehaviour
{
    public Camera shipCamera;
    public Camera planetCamera;

    private bool usingAlt = false;

    void OnEnable()
    {
        shipCamera.enabled = true;
        planetCamera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            usingAlt = !usingAlt;

            shipCamera.enabled = !usingAlt;
            planetCamera.enabled = usingAlt;
        }
    }
}