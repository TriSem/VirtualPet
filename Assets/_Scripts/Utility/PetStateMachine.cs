using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetStateMachine
{
    PetState initialState;
    PetState currentState;

    PetState exitState;

    public void Start()
    {
        currentState = initialState;
    }

    public void Update()
    {
        if(currentState.StateChanged(out PetState newState))
        {
            currentState.OnExit();
            currentState = newState;
            currentState.OnEntry();
        }

        currentState.Update();
    }

    public PetStateMachine(PetState initialState)
    {
        exitState = new PetState();
        currentState = exitState;
    }

    public bool Running => currentState != exitState;
}

public class PetState
{
    List<Transition> transitions = new List<Transition>();

    public bool StateChanged(out PetState newState)
    {
        newState = null;
        foreach(var transition in transitions)
        {
            if(transition.IsTriggered())
            {
                newState = transition.TargetState;
                return true;
            }
        }

        return false;
    }

    public virtual void OnEntry() { }
    public virtual void OnExit() { }
    public virtual void Update() { }
}

public class Transition
{
    List<ICondition> triggerConditions = new List<ICondition>();

    public PetState TargetState { get; private set; }

    public Transition(PetState targetState, List<ICondition> triggerConditions)
    {
        this.TargetState = targetState;
        this.triggerConditions = triggerConditions;
    }

    public bool IsTriggered()
    {
        foreach(var condition in triggerConditions)
        {
            if (!condition.Met)
                return false;
        }
        return true;
    }
}
