using UnityEngine;

public class Grasp : MonoBehaviour
{
    [SerializeField] PetAgent agent = null;
    ActionObject carried = null;

    public bool Carrying => carried != null;

    public void Carry(ActionObject actionObject)
    {
        if (Carrying)
            Release();

        if(actionObject is IPhysicsObject physicsObject)
        {
            physicsObject.Rigidbody.isKinematic = true;
            physicsObject.Collider.enabled = false;
        }

        carried = actionObject;
        carried.transform.parent = transform;
        carried.transform.localPosition = Vector3.zero;
        agent.InternalModel.Add(InternalState.Carrying);
    }

    public void Release()
    {
        if(carried is IPhysicsObject physicsObject)
        {
            physicsObject.Rigidbody.isKinematic = false;
            physicsObject.Collider.enabled = true;
        }

        agent.InternalModel.Remove(InternalState.Carrying);
        carried.transform.parent = null;
        carried = null;
    }
}
