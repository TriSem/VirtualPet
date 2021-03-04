using System;
using System.Collections.Generic;
using UnityEngine;

public class PetStateMachine
{
    PetState initialState;
    PetState currentState;

    public void Start()
    {
        currentState = initialState;
    }

    public void Stop()
    {
        currentState.OnExit();
    }

    public void Update()
    {
        if(currentState.StateChanged(out PetState newState))
        {
            currentState.OnExit();
            currentState = newState;
            currentState.OnEntry();
        }

        currentState.OnUpdate();
    }

    public PetStateMachine(PetState initialState)
    {
        this.initialState = initialState;
    }
}

public class PetState
{
    public List<Transition> Transitions { get; private set; } = new List<Transition>();
    public List<AtomicAction> EntryActions { get; private set; } = new List<AtomicAction>();
    public List<AtomicAction> ExitActions { get; private set; } = new List<AtomicAction>();
    public List<AtomicAction> UpdateActions { get; private set; } = new List<AtomicAction>();

    public bool StateChanged(out PetState newState)
    {
        newState = null;
        foreach(var transition in Transitions)
        {
            if(transition.Triggered)
            {
                newState = transition.TargetState;
                return true;
            }
        }

        return false;
    }

    public void OnEntry() 
    {
        foreach (AtomicAction action in EntryActions)
            action.Use();
    }

    public void OnExit() 
    {
        foreach (AtomicAction action in ExitActions)
            action.Use();
    }

    public void OnUpdate() 
    {
        foreach (AtomicAction action in UpdateActions)
            action.Use();
    }
}

public class Transition
{
    ICondition condition;

    public PetState TargetState { get; private set; }

    public Transition(PetState targetState, ICondition condition)
    {
        this.TargetState = targetState;
        this.condition = condition;
    }

    public bool Triggered => condition.Met;
}