using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionObject : MonoBehaviour
{
    [SerializeField] protected List<Outcome> outcomes = null;

    public ActionStatus Status { get; protected set; }

    public virtual bool IsUsable()
    {
        return Status == ActionStatus.Inactive;
    }

    public abstract void UseAction(PetAgent agent);
    public abstract void Cancel();
}

public enum ActionStatus
{
    Inactive,
    Ongoing,
    Success,
    Cancelled
}
