using UnityEngine;

public class TagTrigger : MonoBehaviour
{
    [SerializeField] string tagName = default;

    public bool Triggered { get; private set; }

    void FixedUpdate()
    {
        Triggered = false;    
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(tagName))
            Triggered = true;
    }
}
