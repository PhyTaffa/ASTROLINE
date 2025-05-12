using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraUIManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;
    [SerializeField] private CinemachineVirtualCamera thirdPersonCam;
    [SerializeField] private GameObject firstPersonUI;
    [SerializeField] private CinemachineBrain cinemachineBrain;

    private Coroutine blendRoutine;

    private void OnEnable()
    {
        cinemachineBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
        firstPersonUI.SetActive(false);
    }

    private void OnDisable()
    {
        cinemachineBrain.m_CameraActivatedEvent.RemoveListener(OnCameraActivated);
    }

    private void OnCameraActivated(ICinemachineCamera fromCam, ICinemachineCamera toCam)
    {
        if (blendRoutine != null)
            StopCoroutine(blendRoutine);
        
        //works in reverser, idk
        if (fromCam == firstPersonCam)
        {
            float blendTime = GetBlendTime(fromCam, toCam);
            blendRoutine = StartCoroutine(ShowUIAfterBlend(blendTime));
        }
        else
        {
            firstPersonUI.SetActive(false);
        }
    }

    private IEnumerator ShowUIAfterBlend(float delay)
    {
        yield return new WaitForSeconds(delay);
        firstPersonUI.SetActive(true);
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