﻿using System.Collections.Generic;
using UnityEngine;

public class PickUp : IntermediaryBehavior
{
    PetStateMachine stateMachine = null;
    [SerializeField] Interaction interaction = null;

    public override void Cancel()
    {
        stateMachine.Stop();
        Status = BehaviorState.Completed;
    }

    public override HashSet<InternalState> GetPredictedChanges()
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
        var exit = new ExitState();
        var toGrab = new Transition(grab, new InteractionCondition(interaction));
        var toExit = new Transition(exit, new MetCondition());

        pursue.Transitions.Add(toGrab);
        grab.Transitions.Add(toExit);
    }

    void Update()
    {
        stateMachine.Update();
    }
}