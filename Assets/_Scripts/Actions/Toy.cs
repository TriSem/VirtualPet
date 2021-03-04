using System.Collections.Generic;
using UnityEngine;

public class Toy : ActionObject
{
    [SerializeField] Interaction interaction = null;
    new Rigidbody rigidbody = null;
    new Collider collider = null;
    PetAgent agent = null;

    public override void Cancel()
    {
    }

    public override void Use(PetAgent agent)
    {
        Status = ActionStatus.Ongoing;
        Pursue();
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
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

    void Pursue()
    {
        agent.Motor.Pursue(transform, 1f);
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

public class PursuitState : PetState
{
    MotorSystem pursuer;
    Transform target;

    public PursuitState(List<Transition> transitions, MotorSystem pursuer, Transform target) : base(transitions)
    {
        this.pursuer = pursuer;
        this.target = target;
    }

    public override void OnExit()
    {
        pursuer.Stop();
    }

    public override void Update()
    {
        pursuer.MoveTo(target.position + target.forward);
    }
}