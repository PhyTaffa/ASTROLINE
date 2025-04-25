using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    void Start()
    {
        //cinemachineCamera = Camera.main.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = cinemachineCamera.transform.position;
        this.transform.rotation = cinemachineCamera.transform.rotation;
        
    }
}
