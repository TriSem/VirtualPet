using UnityEngine;

// Lets the pet carry world objects.
public class PetGrasp : MonoBehaviour
{
    [SerializeField] PetAgent agent = null;
    Behavior carried = null;

    public bool Carrying => carried != null;

    public void Carry(Behavior behavior)
    {
        if (Carrying)
            Release();

        if(behavior is IPhysicsObject physicsObject)
        {
            physicsObject.Rigidbody.isKinematic = true;
            physicsObject.Collider.enabled = false;
        }

        carried = behavior;
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

        Debug.Log("Release");
        agent.InternalModel.Remove(InternalState.Carrying);
        carried.transform.parent = null;
        carried = null;
    }
}
