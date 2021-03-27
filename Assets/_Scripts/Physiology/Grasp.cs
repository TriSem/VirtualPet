using UnityEngine;

public class Grasp : MonoBehaviour
{
    ActionObject carried = null;

    public bool Empty => carried == null;

    public void Carry(ActionObject actionObject)
    {
        if (!Empty)
            Release();

        if(actionObject is IPhysicsObject physicsObject)
        {
            physicsObject.Rigidbody.isKinematic = true;
            physicsObject.Collider.enabled = false;
        }

        carried = actionObject;
        carried.transform.parent = transform;
        carried.transform.localPosition = Vector3.zero;
    }

    public void Release()
    {
        if(carried is IPhysicsObject physicsObject)
        {
            physicsObject.Rigidbody.isKinematic = false;
            physicsObject.Collider.enabled = true;
        }

        carried.transform.parent = null;
        carried = null;
    }
}
