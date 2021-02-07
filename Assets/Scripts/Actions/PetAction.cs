﻿using System.Collections.Generic;
using UnityEngine;

public abstract class PetAction : MonoBehaviour
{
    [SerializeField] protected List<Outcome> baseOutcomes = null;
    [SerializeField] protected PetAgent agent = null;
    [SerializeField] protected List<EventQuery> eventRequirements = new List<EventQuery>();
    [SerializeField] Priority priority = Priority.Regular;
    
    public Priority Priority { get => priority; set => priority  = value; }
    public ActionStatus Status { get; protected set; } = ActionStatus.Inactive;

    public virtual float GetUtility()
    {
        return agent.DriveVector.CalculateUtility(baseOutcomes);
    }

    /// <summary>
    /// Default implementation checks wether all event requirements
    /// have been met or not. Override to fit the function to the
    /// needs of a specific action implementation.
    /// </summary>
    public virtual bool RequirementsMet()
    {
        foreach(var query in eventRequirements)
        {
            if (!agent.EventActive(query))
                return false;
        }
        return true;
    }

    protected abstract void Begin();
    protected abstract void Continue();
    protected abstract void Stop();
}

public enum Priority
{
    Regular,
    Urgent,
    Reflex
}

public enum ActionStatus
{
    Inactive,
    Ongoing,
    Success,
    Failure
}