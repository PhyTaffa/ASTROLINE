using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private LayerMask detectableLayers;
    [SerializeField] private Transform dropOrigin;
    
    private Collider triggerCollider;
    //private HeldItems currentHeldItems = HeldItems.None;

    //private Dictionary<HeldItems, GameObject> heldObjects = new();
    private GameObject heldObject = null;

    
    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        
        //to avoid editor problmes
        triggerCollider.isTrigger = true;
        triggerCollider.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            triggerCollider.enabled = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            triggerCollider.enabled = false;
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            DropFirstHeldItem();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (heldObject != null) return; // Already holding something

        if (((1 << other.gameObject.layer) & detectableLayers) != 0)
        {
            PickupItem item = other.GetComponent<PickupItem>();

            if (item != null)
            {
                TryPickUp(other.gameObject, item);
            }
            else
            {
                Debug.LogWarning($"Object '{other.gameObject.name}' is in detectable layer but missing PickupItem script.");
            }
        }
    }

    private void TryPickUp(GameObject obj, PickupItem item)
    {
        //PickupItem item = obj.GetComponent<PickupItem>();
        if (!HasItem(item.itemType))
        {
            //currentHeldItems |= item.itemType;
            heldObjects[item.itemType] = item.gameObject;
            obj.SetActive(false);
            Debug.Log($"Picked up: {item.itemType}");
        }
        else
        {
            Debug.Log($"Already have: {item.itemType}");
        }
    }
    
    
    private void DropFirstHeldItem()
    {
        foreach (var kvp in heldObjects)
        {
            HeldItems itemType = kvp.Key;
            GameObject obj = kvp.Value;

            // Move to drop position
            Vector3 dropPos = dropOrigin ? dropOrigin.position : transform.position + transform.forward;
            obj.transform.position = dropPos;
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(true);

            // Optionally reset physics
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            heldObjects.Remove(itemType);
            Debug.Log("Dropped: " + itemType);
            break;
        }
    }

    private bool HasItem(HeldItems item)
    {
        return (currentHeldItems & item) != 0;
    }
}