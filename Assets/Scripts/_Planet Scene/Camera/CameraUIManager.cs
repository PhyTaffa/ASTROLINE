using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraUIManager : MonoBehaviour
{
    [Header("1st Person")]
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;
    [SerializeField] private GameObject firstPersonUIRoot; // scannerUIRoot
    
    [Header("3rd Person")]
    [SerializeField] private CinemachineVirtualCameraBase thirdPersonCam;
    [SerializeField] private GameObject thirdPersonUIRoot;
    
    [Header("BrIAn")]
    [SerializeField] private CinemachineBrain cinemachineBrain;
    
    [Header("Post‐Process / VFX Volumes")]
    [SerializeField] private GameObject scannerCamEffect;
    [SerializeField] private GameObject planetCamEffect;

    private Coroutine blendRoutine;

    private void OnEnable()
    {
        cinemachineBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
        
        //we are starting in 3rd person
        firstPersonUIRoot.SetActive(false);
        thirdPersonUIRoot.SetActive(true);
        
        scannerCamEffect.SetActive(false);
        planetCamEffect.SetActive(true);
    }

    private void OnDisable()
    {
        cinemachineBrain.m_CameraActivatedEvent.RemoveListener(OnCameraActivated);
    }

    private void OnCameraActivated(ICinemachineCamera fromCam, ICinemachineCamera toCam)
    {
        if (blendRoutine != null)
            StopCoroutine(blendRoutine);
        
        firstPersonUIRoot.SetActive(false);
        thirdPersonUIRoot.SetActive(false);
        
        scannerCamEffect.SetActive(false);
        planetCamEffect.SetActive(true);

        float blendTime = GetBlendTime(fromCam, toCam);

        if (fromCam == firstPersonCam)
        {
            //show FP UI after the blend
            blendRoutine = StartCoroutine(ShowUIAfterBlend(firstPersonUIRoot, blendTime));
        }
        else if (fromCam == thirdPersonCam)
        {
            //show TP UI immediately
            thirdPersonUIRoot.SetActive(true);
            
            scannerCamEffect.SetActive(false);
            planetCamEffect.SetActive(true);
        }
    }

    private IEnumerator ShowUIAfterBlend(GameObject uiToShow, float delay)
    {
        yield return new WaitForSeconds(delay);
        uiToShow.SetActive(true);
        
        scannerCamEffect.SetActive(true);
        planetCamEffect.SetActive(false);
    }

    private float GetBlendTime(ICinemachineCamera fromCam, ICinemachineCamera toCam)
    {
        if (cinemachineBrain.m_CustomBlends != null)
        {
            string fromName = fromCam?.Name ?? string.Empty;
            string toName = toCam?.Name ?? string.Empty;

            foreach (var blend in cinemachineBrain.m_CustomBlends.m_CustomBlends)
            {
                if (blend.m_From == fromName && blend.m_To == toName)
                {
                    return blend.m_Blend.BlendTime;
                }
            }
        }

        return cinemachineBrain.m_DefaultBlend.BlendTime;
    }
}