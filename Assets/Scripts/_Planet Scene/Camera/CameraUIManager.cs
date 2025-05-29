using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraUIManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;
    [SerializeField] private CinemachineVirtualCamera thirdPersonCam;

    [SerializeField] private GameObject firstPersonUIRoot; // scannerUIRoot
    [SerializeField] private GameObject thirdPersonUIRoot;
    
    [SerializeField] private CinemachineBrain cinemachineBrain;

    private Coroutine blendRoutine;

    private void OnEnable()
    {
        cinemachineBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
        
        //we are starting in 3rd person
        firstPersonUIRoot.SetActive(false);
        thirdPersonUIRoot.SetActive(true);
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
        }
    }

    private IEnumerator ShowUIAfterBlend(GameObject uiToShow, float delay)
    {
        yield return new WaitForSeconds(delay);
        uiToShow.SetActive(true);
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