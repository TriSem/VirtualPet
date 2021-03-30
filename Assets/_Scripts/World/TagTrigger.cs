using UnityEngine;

public class TagTrigger : MonoBehaviour
{
    [SerializeField] string tagName = default;

    public bool Triggered { get; private set; }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
            Triggered = true;
    }

    void OnTriggerExit(Collider other)
    {
        Triggered = false;    
    }
}
