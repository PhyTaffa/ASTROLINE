using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGravity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //faster better because less stuttering
    private float fastRotationSpeed = 500;
    private float gravitylevel = -10;
    
    public void Gravity(Transform worldObjects) {
   
        //Calculating the Gravity Direction
        //Calculate the normalised vector that points any object in the world towards the center of the planet
        //Normalised because it's a sphere and no matter how much we reshape the mesh, the gravity will be spherical from the planets center
        Vector3 vectorPointingObjectTowardCenter = (worldObjects.position - transform.position).normalized;
        
        //Storing the Object's Up Direction
        //Useful so when moving the rotation calculations uses this as a way to make sure the rotation leaves the object in the proper position.
        Vector3 transformUp = worldObjects.up;
       
        //Applying the Gravitational Force
        //this keeps the object on the ground floor
        //remove this for flying ai objects
        worldObjects.GetComponent<Rigidbody>().AddForce(vectorPointingObjectTowardCenter * (gravitylevel));
     
        //Calculating the Target Rotation
        //this way fixes the stutter
        //rotates the character based on the world-position not the character-position based on the world
        Quaternion rotationJourney = Quaternion.FromToRotation(transformUp, vectorPointingObjectTowardCenter) * worldObjects.rotation;
        worldObjects.rotation = Quaternion.Slerp(worldObjects.rotation, rotationJourney, fastRotationSpeed * Time.deltaTime);
    }
    
}
