using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : IntermediaryAction
{
    PetAgent agent;

    public override void Cancel()
    {
        Status = ActionStatus.Completed;
    }

    public override HashSet<InternalState> GetPredictedChanges()
    {
        var changes = new HashSet<InternalState>();
        changes.Add(InternalState.Carrying);
        return changes;
    }

    public override void Use(PetAgent agent)
    {
        this.agent = agent;
        Status = ActionStatus.Ongoing;
    }

    void Update()
    {
        if(Status == ActionStatus.Ongoing)
        {

        }
    }
}
