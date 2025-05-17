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
            DropHeldObject();
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
                PickUpObject(item);
            }
            else
            {
                Debug.LogWarning($"Object '{other.gameObject.name}' is in detectable layer but missing PickupItem script.");
            }
        }
    }

    private void PickUpObject(PickupItem item)
    {
        heldObject = item.gameObject;
        heldObject.SetActive(false);
        Debug.Log("Picked up: " + heldObject.name);
    }

    private void DropHeldObject()
    {
        if (heldObject == null) return;

        Vector3 dropPos = dropOrigin ? dropOrigin.position : transform.position + transform.forward;
        heldObject.transform.position = dropPos;
        heldObject.transform.rotation = Quaternion.identity;
        heldObject.SetActive(true);

        // Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        // if (rb)
        // {
        //     rb.velocity = Vector3.zero;
        //     rb.angularVelocity = Vector3.zero;
        // }

        Debug.Log($"Dropped: {heldObject.name}");
        heldObject = null;
    }

    // private bool HasItem(HeldItems item)
    // {
    //     return (currentHeldItems & item) != 0;
    // }
}