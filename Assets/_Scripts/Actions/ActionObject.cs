using System.Collections.Generic;
using UnityEngine;

public abstract class ActionObject : MonoBehaviour, IAction
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

    public abstract void UseAction(PetAgent agent);

    public abstract void Cancel();

    public bool CanDoBoth(IAction other)
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
