using UnityEngine;

public class Play : ActionObject
{
    [SerializeField] Interaction interaction = null;
    new Rigidbody rigidbody = null;
    new Collider collider = null;
    PetAgent agent = null;
    bool grabbed = false;
    bool shaking = false;
    

    public override void Cancel()
    {
        rigidbody.isKinematic = false;
        collider.enabled = true;
    }

    public override void UseAction(PetAgent agent)
    {
        Status = ActionStatus.Ongoing;
        this.agent = agent;
        agent.Motor.GoInteract(interaction);
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        bool grounded = Physics.Raycast(collider.bounds.center, Vector3.down, collider.bounds.extents.y + 0.01f, 1 << 9);
        if (interaction.PetInRange && agent != null && grounded)
        {
            rigidbody.isKinematic = true;
            collider.enabled = false;
            transform.parent = agent.transform;
            grabbed = true;
        }
        if(grabbed && !shaking)
        {
            float random = Random.Range(0, 1f);
            if (random <= 0.5f)
                Shake();
            else
                Throw();
        }
    }

    void Shake()
    {
    }

    void Throw()
    {
        transform.parent = null;
        rigidbody.isKinematic = false;
        collider.enabled = true;
        rigidbody.velocity = agent.transform.forward + Vector3.up;
        grabbed = false;
        Cancel();
    }
}
