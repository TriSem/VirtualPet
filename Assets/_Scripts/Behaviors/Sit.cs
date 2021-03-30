using System.Collections.Generic;
using UnityEngine;

public class Sit : Behavior, IAudioReciever
{
    [SerializeField, Range(1f, 100f)]
    float minimumSitTime = 1f;

    [SerializeField, Range(1f, 100f)]
    float maximumSitTime = 5f;

    [SerializeField]
    TagTrigger tagTrigger = null;

    bool learned = false;
    bool learningEnabled = false;

    [SerializeField] Transform icon = null;
    [SerializeField] AudioHub audioHub = null;

    TimerCondition timerCondition = new TimerCondition(0f);

    public override void Cancel()
    {
        
    }

    public void RecieveSignal(Command command)
    {
        if (learned && command == Command.Sit)
        {
            Debug.Log("Sit understood.");
        }
        if (learningEnabled && command == Command.Sit)
        {
        }
    }

    void Start()
    {
        audioHub.Register(this);

        var exit = new ExitState();
        var sit = new SitState();
        var toExit = new Transition(exit, timerCondition);

        sit.Transitions.Add(toExit);
    }

    public override void Use(PetAgent agent)
    {
        Status = BehaviorState.Ongoing;
        if(!learned)
        {
            learningEnabled = true;
            icon.gameObject.SetActive(true);
        }

        var timer = 
        sit.Transitions = new Transition();
    }

    void Update()
    {
        
    }
}

public class SitState : PetState
{
    public override void OnEntry(PetAgent agent, Behavior behavior)
    {
        agent.Motor.SitDown();
    }

    public override void OnExit(PetAgent agent, Behavior behavior)
    {
        agent.Motor.GetUp();
    }
}

