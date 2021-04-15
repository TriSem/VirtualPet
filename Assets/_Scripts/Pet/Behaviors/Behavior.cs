using System.Collections.Generic;
using UnityEngine;

public abstract class Behavior : MonoBehaviour, IBehaviour
{
    // Predicts how the pets drives will change when using this action.
    [SerializeField] 
    protected List<Outcome> outcomes = null;
    
    public BehaviorState Status { get; protected set; } = BehaviorState.Inactive;

    public virtual bool PreconditionsMet(InternalModel internalModel) => true;

    public  float CalculateUtility(DriveVector drives) => drives.CalculateUtility(outcomes);

    public abstract void Use(PetAgent agent);

    public abstract void Cancel();
}

public enum BehaviorState
{
    Inactive,
    Ongoing
}

public interface IPhysicsObject
{
    Rigidbody Rigidbody { get; }
    Collider Collider { get; }
}