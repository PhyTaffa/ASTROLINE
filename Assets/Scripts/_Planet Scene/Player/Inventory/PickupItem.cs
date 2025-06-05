using UnityEngine;

public class PickupItem : MonoBehaviour
{
    //was used, but, respectfully, frick you
    public HeldItems itemType;
    
    [SerializeField] private GameObject scannableToSpawn;
    [SerializeField] private bool wasEverePickedUp = false;

    private void Start()
    {
        if (scannableToSpawn == null)
        {
            //will not spawn anything
            wasEverePickedUp = true;
            
            //to avoid anymore issue about missing reference
            scannableToSpawn = gameObject;
        }
    }
    
    public void SpawnMfUponBeignPickedUp()
    {
        if (!wasEverePickedUp)
        {
            wasEverePickedUp = true;
            
            //instanciate the scannable
            //float offSet = scannableToSpawn.GetComponent<Collider>()./2;
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Instantiate(scannableToSpawn, spawnPos, Quaternion.identity);
            Debug.Log($"{this.gameObject.name} beign picked up, spawend: {scannableToSpawn.name}");
        }
    }
}