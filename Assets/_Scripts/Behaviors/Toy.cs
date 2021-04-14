using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Toy : Behavior, IPhysicsObject
{
    [SerializeField] Interaction interaction = null;
    PetStateMachine stateMachine = null;

    public Rigidbody Rigidbody { get; private set; } = null;

    public Collider Collider { get; private set; } = null;

    public override void Cancel()
    {
        stateMachine.Stop();
        Status = BehaviorState.Inactive;
    }

    public override void Use(PetAgent agent)
    {
        Status = BehaviorState.Ongoing;
        stateMachine.Start(agent, this);
    }

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();

        var pursuit = new PursueState();
        var playAndWander = new PlayAndWander();
        var canInteract = new InteractionCondition(interaction);
        pursuit.Transitions.Add(new Transition(playAndWander, canInteract));
        stateMachine = new PetStateMachine(pursuit);
    }

    void Update()
    {
        stateMachine.Update();
    }
}

public class PlayAndWander : PetState
{
    float nextShakeTime;

    public override void OnEntry(PetAgent agent, Behavior behavior)
    {
        agent.Motor.Wander();
        agent.Mouth.Carry(behavior);
        agent.Motor.StartWaggingTail();
        nextShakeTime = Time.time + Random.Range(1f, 5f);
    }

    public override void OnExit(PetAgent agent, Behavior behavior)
    {
        agent.Motor.Stop();
        agent.Motor.StopWaggingTail();
        agent.Mouth.Release();
    }

    public override void OnUpdate(PetAgent agent, Behavior behavior)
    {
        float time = Time.time;
        if (time > nextShakeTime)
        {
            agent.Motor.ShakeHead();
            nextShakeTime = time + Random.Range(1f, 5f);
        }
    }
}