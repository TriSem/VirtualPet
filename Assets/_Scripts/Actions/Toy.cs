using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Toy : ActionObject, IPhysicsObject
{
    [SerializeField] Interaction interaction = null;
    PetStateMachine stateMachine = null;

    public Rigidbody Rigidbody { get; private set; } = null;

    public Collider Collider { get; private set; } = null;

    public override void Cancel()
    {
        stateMachine.Stop();
        Status = ActionStatus.Inactive;
        Debug.Log("Toy stop.");
    }

    public override void Use(PetAgent agent)
    {
        Status = ActionStatus.Ongoing;
        stateMachine.Start(agent, this);
        Debug.Log("Toy start");
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

public class PursueState : PetState
{
    public override void OnEntry(PetAgent agent, ActionObject actionObject)
    {
        agent.Motor.Pursue(actionObject.transform, 0f);
        Debug.Log("Enter pursuit.");
    }

    public override void OnExit(PetAgent agent, ActionObject actionObject)
    {
        agent.Motor.Stop();
        Debug.Log("Leave pursuit.");
    }

    public override void OnUpdate(PetAgent agent, ActionObject actionObject)
    {
    }
}

public class PlayAndWander : PetState
{
    float nextShakeTime;

    public override void OnEntry(PetAgent agent, ActionObject actionObject)
    {
        agent.Motor.Wander();
        agent.Snoot.Carry(actionObject);
        nextShakeTime = Time.time + Random.Range(1f, 5f);
        Debug.Log("Enter play and wander.");
    }

    public override void OnExit(PetAgent agent, ActionObject actionObject)
    {
        agent.Motor.Stop();
        agent.Snoot.Release();
        Debug.Log("Leave play and wander.");
    }

    public override void OnUpdate(PetAgent agent, ActionObject actionObject)
    {
        float time = Time.time;
        if(time > nextShakeTime)
        {
            // Play shake animation
            nextShakeTime = time + Random.Range(1f, 5f);
        }
    }
}