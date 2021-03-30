using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingPlace : Behavior
{
    [SerializeField] PetStateMachine stateMachine;

    public override void Cancel()
    {
        
    }

    public override void Use(PetAgent agent)
    {
    }

    void Start()
    {
    }

    void Update()
    {
        stateMachine.Update();
    }
}

public class LieDownState : PetState
{
    public override void OnEntry(PetAgent agent, Behavior behavior)
    {
        agent.Motor.LieDown();
    }

    public override void OnExit(PetAgent agent, Behavior behavior)
    {
        agent.Motor.GetUp();
    }

    public override void OnUpdate(PetAgent agent, Behavior behavior)
    {
        throw new System.NotImplementedException();
    }
}

