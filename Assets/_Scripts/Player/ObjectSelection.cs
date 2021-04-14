using UnityEngine;

// Lets the player select and pick up objects.
public class ObjectSelection : MonoBehaviour
{
    [SerializeField] 
    new Camera camera = null;

    [SerializeField] 
    float selectionDistance = 20f;
    
    [SerializeField] 
    float grabDistance = 1f;

    [SerializeField] float throwSpeed = 1f;

    [SerializeField] 
    Transform selectionMarker = null;

    [SerializeField, Tooltip("Determines, were held objects will be placed")] 
    Transform objectHold = null;

    WorldObject currentlySelected = null;
    WorldObject currentlyHeld = null;
    LayerMask mask;

    public bool ItemSelected => currentlySelected != null;
    public bool HoldingItem => currentlyHeld != null;

    void Start()
    {
        selectionMarker = Instantiate(selectionMarker);
        selectionMarker.gameObject.SetActive(false);
        mask = LayerMask.GetMask("WorldObject");
    }

    void Update()
    {
        var forward = camera.transform.forward;
        var origin = camera.transform.position;
        var ray = new Ray(origin, forward);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, selectionDistance, mask))
        {
            currentlySelected = hitInfo.transform.gameObject.GetComponent<WorldObject>();
            selectionMarker.gameObject.SetActive(true);
            selectionMarker.position = currentlySelected.transform.position;
        }
        else
        {
            currentlySelected = null;
            selectionMarker.gameObject.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && CanPickUpSelection())
        {
            Grab();
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0) && currentlyHeld)
        {
            Release();
        }
        else if(Input.GetKeyDown(KeyCode.T) && currentlyHeld)
        {
            Throw();
        }
    }

    bool CanPickUpSelection() => ItemSelected && !HoldingItem && WithinGrabbingDistance();

    bool WithinGrabbingDistance() => Vector3.Distance(transform.position, currentlySelected.transform.position) <= grabDistance;

    // Disable any physics on an object and put it in the players hand
    void Grab()
    {
        currentlyHeld = currentlySelected;
        var collider = currentlyHeld.GetComponent<Collider>();
        var rigidbody = currentlyHeld.GetComponent<Rigidbody>();

        // Only objects with colliders can be selected, so we don't need to null check.
        collider.enabled = false;
        if (rigidbody != null)
            rigidbody.isKinematic = true;

        currentlyHeld.transform.parent = objectHold;
        currentlyHeld.transform.localPosition = Vector3.zero;
    }

    // Reenable any physics on an object and let it fall down.
    void Release()
    {
        var collider = currentlyHeld.GetComponent<Collider>();
        var rigidbody = currentlyHeld.GetComponent<Rigidbody>();

        collider.enabled = true;
        if (rigidbody != null)
            rigidbody.isKinematic = false;

        currentlyHeld.transform.parent = null;
        currentlyHeld.transform.position = objectHold.transform.position;
        currentlyHeld = null;
    }

    // Release and propel the object forward.
    public void Throw()
    {
        var rigidBody = currentlyHeld.GetComponent<Rigidbody>();
        if (rigidBody != null)
            rigidBody.velocity = camera.transform.forward * throwSpeed;

        Release();
    }

    public void PlaceObjectIntoHand(WorldObject worldObject)
    {
        if (currentlyHeld)
            Release();
        currentlySelected = worldObject;
        Grab();
    }
}
