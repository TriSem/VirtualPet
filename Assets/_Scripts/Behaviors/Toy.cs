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
        Debug.Log("Toy stop.");
    }

    public override void Use(PetAgent agent)
    {
        Status = BehaviorState.Ongoing;
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