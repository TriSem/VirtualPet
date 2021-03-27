using UnityEngine;

public class Interaction : MonoBehaviour
{
    new SphereCollider collider = default;

    public bool PlayerInRange { get; private set; } = false;
    public bool PetInRange { get; private set; }

    void Start()
    {
        collider = GetComponent<SphereCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pet"))
            PetInRange = true;
        else if (other.CompareTag("Player"))
            PlayerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pet"))
            PetInRange = false;
        else if (other.CompareTag("Player"))
            PlayerInRange = false;
    }

    public float InteractionRadius => collider.radius;
}
