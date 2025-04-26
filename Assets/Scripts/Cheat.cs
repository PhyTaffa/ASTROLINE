using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    [SerializeField] private Transform one;
    [SerializeField] private Transform second;
    [SerializeField] private Transform three;
    [SerializeField] private Transform four;
    [SerializeField] private Transform five;
    [SerializeField] private Transform six;
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
            playerTransform.position = one.position;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerTransform.position = second.position;
        }        
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerTransform.position = three.position;
        }        
        if (Input.GetKeyDown(KeyCode.B))
        {
            playerTransform.position = four.position;
        }        
        if (Input.GetKeyDown(KeyCode.J))
        {
            playerTransform.position = five.position;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            playerTransform.position = six.position;
        }
            
    }
}
