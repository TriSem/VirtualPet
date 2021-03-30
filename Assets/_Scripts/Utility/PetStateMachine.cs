using System.Collections.Generic;
using UnityEngine;

public class PetStateMachine
{

    PetState initialState;
    PetState currentState;
    PetAgent agent;
    Behavior behavior;
    bool running = false;

    public void Start(PetAgent agent, Behavior behavior)
    {
        running = true;
        currentState = initialState;
        currentState.OnEntry(agent, behavior);
        this.agent = agent;
        this.behavior = behavior;
    }

    public void Stop()
    {
        currentState.OnExit(agent, behavior);
        running = false;
    }

    public void Update()
    {
        if (!running)
            return;

        if(currentState.StateChanged(out PetState newState))
        {
            currentState.OnExit(agent, behavior);
            currentState = newState;
            currentState.OnEntry(agent, behavior);
        }

        currentState.OnUpdate(agent, behavior);
    }

    public PetStateMachine(PetState initialState)
    {
        this.initialState = initialState;
    }
}

public abstract class PetState
{
    public static readonly ExitState ExitState = new ExitState();

    public List<Transition> Transitions { get; private set; } = new List<Transition>();

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

    public virtual void OnEntry(PetAgent agent, Behavior behavior) { }

    public virtual void OnExit(PetAgent agent, Behavior behavior) { }

    public virtual void OnUpdate(PetAgent agent, Behavior behavior) { }
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

public class ExitState : PetState
{
    public override void OnEntry(PetAgent agent, Behavior behavior)
    {
        behavior.Cancel();
    }
}

public class PlayAndWander : PetState
{
    float nextShakeTime;

    public override void OnEntry(PetAgent agent, Behavior behavior)
    {
        agent.Motor.Wander();
        agent.Snoot.Carry(behavior);
        nextShakeTime = Time.time + Random.Range(1f, 5f);
        Debug.Log("Enter play and wander.");
    }

    public override void OnExit(PetAgent agent, Behavior behavior)
    {
        agent.Motor.Stop();
        agent.Snoot.Release();
        Debug.Log("Leave play and wander.");
    }

    public override void OnUpdate(PetAgent agent, Behavior behavior)
    {
        float time = Time.time;
        if (time > nextShakeTime)
        {
            // Play shake animation
            nextShakeTime = time + Random.Range(1f, 5f);
        }
    }
}

public class PursueState : PetState
{
    public override void OnEntry(PetAgent agent, Behavior behavior)
    {
        agent.Motor.Pursue(behavior.transform, 0f);
        Debug.Log("Enter pursuit.");
    }

    public override void OnExit(PetAgent agent, Behavior behavior)
    {
        agent.Motor.Stop();
        Debug.Log("Leave pursuit.");
    }
}

public class GrabState : PetState
{
    public override void OnEntry(PetAgent agent, Behavior behavior)
    {
        agent.Snoot.Carry(behavior);
    }
}

public class FollowState : PetState
{
    public override void OnEntry(PetAgent agent, Behavior behavior)
    {
        agent.Motor.Follow(behavior.transform);
    }

    public override void OnExit(PetAgent agent, Behavior behavior)
    {
        agent.Motor.Stop();
    }
}

public class DropState : PetState
{
    public override void OnEntry(PetAgent agent, Behavior behavior)
    {
        agent.Snoot.Release();
    }
}
