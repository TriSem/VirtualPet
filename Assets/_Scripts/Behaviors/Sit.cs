using UnityEngine;

public class Sit : Behavior, IAudioReciever
{
    [SerializeField, Range(1f, 100f)]
    float minimumSitTime = 1f;

    [SerializeField, Range(1f, 100f)]
    float maximumSitTime = 5f;

    [SerializeField] AudioHub audioHub = null;

    PetStateMachine stateMachine = null;
    TimerCondition timerCondition = new TimerCondition(0f);

    public override void Cancel()
    {
        Status = BehaviorState.Inactive;
        stateMachine.Stop();
    }

    public void RecieveSignal(Command command)
    {
        
    }

    void Start()
    {
        audioHub.Register(this);

        var exit = new ExitState();
        var sit = new SitState();
        var toExit = new Transition(exit, timerCondition);

        sit.Transitions.Add(toExit);

        stateMachine = new PetStateMachine(sit);
    }

    public override void Use(PetAgent agent)
    {
        Status = BehaviorState.Ongoing;

        timerCondition.StopTime = Random.Range(minimumSitTime, maximumSitTime) + Time.time;
        stateMachine.Start(agent, this);
    }

    void Update()
    {
        stateMachine?.Update();
    }
}

public class SitState : PetState
{
    public override void OnEntry(PetAgent agent, Behavior behavior)
    {
        agent.Motor.SitDown();
        agent.Learning.StartLearning("Sit");
    }

    public override void OnExit(PetAgent agent, Behavior behavior)
    {
        agent.Motor.GetUp();
        agent.Learning.StopLearning();
    }
}

