using System.Collections.Generic;
using UnityEngine;

public class Toy : ActionObject, IPhysicsObject
{
    [SerializeField] Interaction interaction = null;
    new Rigidbody rigidbody = null;
    new Collider collider = null;
    PetAgent agent = null;

    public Rigidbody Rigidbody => rigidbody;

    public Collider Collider => collider;

    public override void Cancel()
    {
    }

    public override void Use(PetAgent agent)
    {
        Status = ActionStatus.Ongoing;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    void AssembleStateMachine(PetAgent agent)
    {
        var pursue = new PursuitAction(agent, this);
        var grab = new GrabAction(agent, this);
        var release = new ReleaseAction(agent, this);

        var pursuitState = new PetState();
        var carryState = new PetState();

        var canGrabToy = new InteractionCondition(interaction);

        pursuitState.Transitions.Add(new Transition(carryState, canGrabToy));
        pursuitState.EntryActions.Add(pursue);
    }

    void Update()
    {
    }

    public void SetPhysicsEnabled(bool enabled)
    {
        if(enabled)
        {
            collider.enabled = true;
            rigidbody.isKinematic = false;
        }
        else
        {
            collider.enabled = false;
            rigidbody.isKinematic = true;
        }
    }

    public override bool IsUsable()
    {
        return rigidbody.velocity.magnitude < 1f;
    }
    bool CanGrabBall()
    {
        return interaction.PetInRange;
    }

    void Grab()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        transform.parent = agent.Snoot.transform;
        transform.localPosition = agent.Snoot.transform.position;
    }

    void Throw()
    {
        transform.parent = null;
        rigidbody.isKinematic = false;
        collider.enabled = true;
        rigidbody.velocity = agent.transform.forward + Vector3.up;
    }
}