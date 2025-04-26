using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject vCamGO;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    private Quaternion vcamRot;
    
    void Start()
    {
        //vCamGO = cinemachineCamera.GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 euler = vCamGO.transform.rotation.eulerAngles; 
        vcamRot = vCamGO.transform.rotation;
        Debug.Log(euler.x);
        // vcamRot.y = 0;
        // vcamRot.z = 0;
        // vcamRot.x *= -1;
        //this.transform.rotation =  Quaternion.Euler(-euler.x, 0f, 0f);
        //this.transform.rotation.x = vcamRot.x * -1;
    }
}
