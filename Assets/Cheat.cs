using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    [SerializeField] private Transform Top;
    [SerializeField] private Transform Bottom;
    [SerializeField] private Transform Right;
    [SerializeField] private Transform Left;
    
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerTransform.position = Top.position;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerTransform.position = Bottom.position;
        }        
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerTransform.position = Right.position;
        }        
        if (Input.GetKeyDown(KeyCode.B))
        {
            playerTransform.position = Left.position;
        }
            
    }
}
