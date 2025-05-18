using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    [SerializeField] private LayerMask detectableLayers;
    [SerializeField] private Transform dropOrigin;
    
    [Header("UI Preview")]
    [SerializeField] private Transform previewHolder;
    [SerializeField] private Camera previewCamera;
    [SerializeField] private RawImage previewImage;
    
    private Collider triggerCollider;
    //private HeldItems currentHeldItems = HeldItems.None;

    //private Dictionary<HeldItems, GameObject> heldObjects = new();
    private GameObject heldObject = null;
    private GameObject heldPreviewClone = null;

    private NotebookPages notebook;
    
    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        
        //to avoid editor problmes
        triggerCollider.isTrigger = true;
        triggerCollider.enabled = false;
        
        if (previewImage != null)
            previewImage.enabled = false;
        
        notebook = FindObjectOfType<NotebookPages>();
        if (notebook == null)
        {
            Debug.LogError("Notebook Pages could not be found");
        }
    }

    private void Update()
    {
        if (notebook != null)// .!NotebookOpen to avoid picking and dropping while in notebook
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
        
        ShowHeldItemPreview(heldObject);
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
        
        HideHeldItemPreview();
    }
    
    private void ShowHeldItemPreview(GameObject original)
    {
        if (heldPreviewClone != null)
            Destroy(heldPreviewClone);

        heldPreviewClone = Instantiate(original, previewHolder);
        heldPreviewClone.layer = LayerMask.NameToLayer("Preview");
        foreach (Transform t in heldPreviewClone.GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.layer = LayerMask.NameToLayer("Preview");
        }

        heldPreviewClone.transform.localPosition = Vector3.zero;
        heldPreviewClone.transform.localRotation = Quaternion.identity;
        heldPreviewClone.transform.localScale = Vector3.one;

        if (previewImage != null)
            previewImage.enabled = true;
        
        heldPreviewClone.SetActive(true);
    }

    private void HideHeldItemPreview()
    {
        if (heldPreviewClone != null)
            Destroy(heldPreviewClone);

        if (previewImage != null)
            previewImage.enabled = false;
    }

    // private bool HasItem(HeldItems item)
    // {
    //     return (currentHeldItems & item) != 0;
    // }
}