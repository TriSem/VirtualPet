using System.Collections.Generic;
using UnityEngine;

public abstract class ActionObject : MonoBehaviour, IBehaviour
{
    [SerializeField] protected List<Outcome> outcomes = null;

    public ActionStatus Status { get; protected set; } = ActionStatus.Inactive;

    public virtual bool PreconditionsMet()
    {
        return true;
    }

    public virtual bool IsUsable()
    {
        return Status == ActionStatus.Inactive;
    }

    public virtual float CalculateUtility(DriveVector drives)
    {
        return drives.CalculateUtility(outcomes);
    }

    public abstract void Use(PetAgent agent);

    public abstract void Cancel();

    public bool CanDoBoth(IBehaviour other)
    {
        return false;
    }

    public virtual bool Interruptible() => true;
}

public enum ActionStatus
{
    Inactive,
    Ongoing,
}

public interface IPhysicsObject
{
    Rigidbody Rigidbody { get; }
    Collider Collider { get; }
}

public abstract class AtomicAction
{
    protected PetAgent agent;
    protected ActionObject actionObject;

    public AtomicAction(PetAgent agent, ActionObject actionObject)
    {
        this.agent = agent;
        this.actionObject = actionObject;
    }

    public abstract void Use();
}

public class PursuitAction : AtomicAction
{
    public PursuitAction(PetAgent agent, ActionObject actionObject) : base(agent, actionObject)
    {
    }

    public override void Use()
    {
        agent.Motor.Pursue(actionObject.transform, 1f);
    }
}

public class GrabAction : AtomicAction
{
    public GrabAction(PetAgent agent, ActionObject actionObject) : base(agent, actionObject)
    {
    }

    public override void Use()
    {
        actionObject.transform.parent = agent.transform;
        actionObject.transform.localPosition = Vector3.zero;
        if(actionObject is IPhysicsObject physicsObject)
        {
            physicsObject.Rigidbody.isKinematic = true;
            physicsObject.Collider.enabled = false;
        }    
    }
}

public class ReleaseAction : AtomicAction
{
    public ReleaseAction(PetAgent agent, ActionObject actionObject) : base(agent, actionObject)
    {
    }

    public override void Use()
    {
        actionObject.transform.parent = null;
        if(actionObject is IPhysicsObject physicsObject)
        {
            physicsObject.Collider.enabled = true;
            physicsObject.Rigidbody.isKinematic = false;
        }
    }
}
