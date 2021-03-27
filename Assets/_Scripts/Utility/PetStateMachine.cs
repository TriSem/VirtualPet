using System.Collections.Generic;

public class PetStateMachine
{
    PetState initialState;
    PetState currentState;
    PetAgent agent;
    ActionObject actionObject;
    bool running = false;

    public void Start(PetAgent agent, ActionObject actionObject)
    {
        running = true;
        currentState = initialState;
        currentState.OnEntry(agent, actionObject);
        this.agent = agent;
        this.actionObject = actionObject;
    }

    public void Stop()
    {
        currentState.OnExit(agent, actionObject);
        running = false;
    }

    public void Update()
    {
        if (!running)
            return;

        if(currentState.StateChanged(out PetState newState))
        {
            currentState.OnExit(agent, actionObject);
            currentState = newState;
            currentState.OnEntry(agent, actionObject);
        }

        currentState.OnUpdate(agent, actionObject);
    }

    public PetStateMachine(PetState initialState)
    {
        this.initialState = initialState;
    }
}

public abstract class PetState
{
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

    public abstract void OnEntry(PetAgent agent, ActionObject actionObject);

    public abstract void OnExit(PetAgent agent, ActionObject actionObject);

    public abstract void OnUpdate(PetAgent agent, ActionObject actionObject);
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
    public override void OnEntry(PetAgent agent, ActionObject actionObject)
    {
        actionObject.Cancel();
    }

    public override void OnExit(PetAgent agent, ActionObject actionObject)
    {
    }

    public override void OnUpdate(PetAgent agent, ActionObject actionObject)
    {
    }
}