using System.Collections.Generic;
using UnityEngine;

public class PickUp : Behavior, IIntermediary
{
    PetStateMachine stateMachine = null;
    [SerializeField] Interaction interaction = null;

    public override void Cancel()
    {
        stateMachine.Stop();
        Status = BehaviorState.Inactive;
    }

    public HashSet<InternalState> GetPredictedChanges()
    {
        var changes = new HashSet<InternalState>();
        changes.Add(InternalState.Carrying);
        return changes;
    }

    public override void Use(PetAgent agent)
    {
        Status = BehaviorState.Ongoing;
        stateMachine.Start(agent, this);
    }

    void Start()
    {
        var pursue = new PursueState();
        var grab = new GrabState();
        var toGrab = new Transition(grab, new InteractionCondition(interaction));
        var toExit = new Transition(PetState.ExitState, new MetCondition());

        pursue.Transitions.Add(toGrab);
        grab.Transitions.Add(toExit);
    }

    void Update()
    {
        stateMachine.Update();
    }
}
