using UnityEngine;

public class Kick : MonoBehaviour
{
    [SerializeField] float strength = 1f;

    void OnTriggerEnter(Collider other)
    { 
        if(other.attachedRigidbody != null)
        {
            var direction = other.transform.position - transform.position;
            direction.y = 1f;
            direction.Normalize();
            other.attachedRigidbody.AddForce(direction * strength);
        }
    }
}
