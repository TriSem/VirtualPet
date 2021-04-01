using UnityEngine;

public class TouchReceptor : MonoBehaviour
{
    public bool Fired { get; private set; }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Hand"))
        {
            Debug.Log("Touch registered");
            Fired = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Hand"))
        {
            Debug.Log("Touch lost");
            Fired = false;
        }
    }
}
