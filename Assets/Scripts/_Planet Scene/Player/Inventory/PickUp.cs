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

    [Header("Animations")]
    [SerializeField] private PlayerAnimator playerAnimator;

    private Collider triggerCollider;
    private GameObject heldObject = null;
    private int originalLayer = -1;

    private NotebookPages notebook;

    public event System.Action OnPickedUp;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
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
        if (notebook)
        {
            if (Input.GetMouseButtonDown(0))
            {
                triggerCollider.enabled = true;
                playerAnimator.SetAnimationState(EPlayerAnimatorStates.Grab);
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
        if (heldObject != null) return;

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
        item.SpawnMfUponBeignPickedUp();
        originalLayer = heldObject.layer;

        ShowHeldItemPreview(heldObject);
        Debug.Log($"Picked up: {heldObject.name}");
    }

    private void DropHeldObject()
    {
        if (heldObject == null) return;

        HideHeldItemPreview();

        heldObject.transform.SetParent(null);
        heldObject.transform.position = dropOrigin ? dropOrigin.position : transform.position + transform.forward;
        heldObject.transform.rotation = Quaternion.identity;

        Debug.Log($"Dropped: {heldObject.name}");
        heldObject = null;
        originalLayer = -1;
    }

    private void ShowHeldItemPreview(GameObject obj)
    {
        obj.transform.SetParent(previewHolder);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        int previewLayer = LayerMask.NameToLayer("Preview");
        SetLayerRecursively(obj, previewLayer);

        if (previewImage != null)
            previewImage.enabled = true;
    }

    private void HideHeldItemPreview()
    {
        heldObject.transform.SetParent(null);
        SetLayerRecursively(heldObject, originalLayer);

        if (previewImage != null)
            previewImage.enabled = false;
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform t in obj.GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.layer = newLayer;
        }
    }
}
