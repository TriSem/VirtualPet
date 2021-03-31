using System.Collections.Generic;
using UnityEngine;

public class Bed : Behavior, IIntermediary
{
    PetStateMachine stateMachine;

    public override void Cancel()
    {
        stateMachine.Stop();
        Status = BehaviorState.Inactive;
    }

    public HashSet<InternalState> GetPredictedChanges()
    {
        var changes = new HashSet<InternalState>();
        changes.Add(InternalState.InBed);
        return changes;
    }

    public override void Use(PetAgent agent)
    {
        Status = BehaviorState.Ongoing;

        var moveTo = new MoveToState();
        var sit = new SitState();
        var toSit = new Transition(sit, new DestinationCondition(agent.Motor));
        sit.Transitions.Add(toSit);
        stateMachine = new PetStateMachine(moveTo);
        stateMachine.Start(agent, this);
    }

    void Update()
    {
        stateMachine?.Update();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pet"))
        {
            other.GetComponentInParent<PetAgent>().InternalModel.Add(InternalState.InBed);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Pet"))
        {
            other.GetComponentInParent<PetAgent>().InternalModel.Remove(InternalState.InBed);
        }
    }
}