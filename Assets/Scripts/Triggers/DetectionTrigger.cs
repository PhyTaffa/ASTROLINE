using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask givenLayer;
    [SerializeField] private GameObject detectionObjectToSpawn;
    private bool instanciatedAlready = false;
    [SerializeField] private float offsetValue = 1f;
    private void OnTriggerEnter(Collider other)
    {
        //chekc if on good layer
        if (((1 << other.gameObject.layer) & givenLayer) != 0)
        {
            //if not instanciated yet
            if (!instanciatedAlready)
            {
                instanciatedAlready = true;
                

                //assoomsworld is centered a 0,0,0
                Vector3 surfaceNormal = (other.transform.position - Vector3.zero).normalized;

                #region Test for feature

                    float capsuleHeight = detectionObjectToSpawn.GetComponent<CapsuleCollider>().height/2;
                    Vector3 spawnPos = other.transform.position + surfaceNormal * (offsetValue + capsuleHeight);
    
                    //yaws towards the object that triggered the spawn
                    Vector3 toTarget = (other.transform.position - spawnPos).normalized;
    
                    //orthogonal projection
                    Vector3 tangentDirection = Vector3.ProjectOnPlane(toTarget, surfaceNormal).normalized;

                #endregion

                #region Spawn, move out of the cave -> move towards object
                
                    
                
                #endregion

                Quaternion rotation = Quaternion.LookRotation(tangentDirection, surfaceNormal);

                Instantiate(detectionObjectToSpawn, transform.position, rotation);
                Debug.Log($"{this.gameObject.name} beign picked up, spawend: {detectionObjectToSpawn.name}");
            }
        }
    }
}
