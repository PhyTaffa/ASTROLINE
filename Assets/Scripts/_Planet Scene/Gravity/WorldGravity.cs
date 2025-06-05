using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGravity : MonoBehaviour
{
    //faster better because less stuttering
    private float fastRotationSpeed = 500;
    private float gravitylevel = -10;
    
    public void Gravity(Transform worldObjectTransform, Rigidbody worldObjectRB,  bool isFlying) {
   
        //Calculating the Gravity Direction
        //Calculate the normalised vector that points any object in the world towards the center of the planet
        //Normalised because it's a sphere and no matter how much we reshape the mesh, the gravity will be spherical from the planets center
        Vector3 vectorPointingObjectTowardCenter = (worldObjectTransform.position - transform.position).normalized;
        
        //Storing the Object's Up Direction
        //Useful so when moving the rotation calculations uses this as a way to make sure the rotation leaves the object in the proper position.
        Vector3 transformUp = worldObjectTransform.up;
       
        //Applying the Gravitational Force
        //this keeps the object on the ground floor
        //remove this for flying ai objects
        if (!isFlying) {
            worldObjectRB.AddForce(vectorPointingObjectTowardCenter * gravitylevel);
            //worldObjectTransform.GetComponent<Rigidbody>().AddForce(vectorPointingObjectTowardCenter * gravitylevel);
        }
        
        //Calculating the Target Rotation
        //this way fixes the stutter
        //rotates the character based on the world-position not the character-position based on the world
        //Quaternion rotationJourney = Quaternion.FromToRotation(transformUp, vectorPointingObjectTowardCenter) * worldObjectTransform.rotation;
        //worldObjectTransform.rotation = Quaternion.Slerp(worldObjectTransform.rotation, rotationJourney, fastRotationSpeed * Time.deltaTime);
        
        
        float angleDifferenceDegree = Vector3.Angle(transformUp, vectorPointingObjectTowardCenter);
        float rotationThreshold = 0.1f;

        if (angleDifferenceDegree > rotationThreshold)
        {
            Quaternion rotationJourney = Quaternion.FromToRotation(transformUp, vectorPointingObjectTowardCenter) * worldObjectTransform.rotation;
            worldObjectTransform.rotation = Quaternion.Slerp(worldObjectTransform.rotation, rotationJourney, fastRotationSpeed * Time.deltaTime);
        }
        
    }
    
}
