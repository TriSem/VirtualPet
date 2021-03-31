using UnityEngine;

public class TouchReceptor : MonoBehaviour
{
    public bool Fired { get; private set; }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            Fired = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Fired = false;
        }
    }
}
