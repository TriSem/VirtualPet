using UnityEngine;

public class EatTreat : Behavior, IPhysicsObject
{
    [SerializeField, Range(0f, 1000f)] 
    float fillValue = 5f;

    [SerializeField] float timeToChew = 2f;

    [SerializeField] Interaction interaction = default;
    [SerializeField] Material highlightMaterial = default;

    new Rigidbody rigidbody;
    new Collider collider;
    new MeshRenderer renderer= null;
    Material originalMaterial = null;

    public bool Eaten { get; set; } = false;

    PetStateMachine stateMachine;

    public float FillValue => fillValue;

    public Rigidbody Rigidbody => rigidbody;

    public Collider Collider => collider;

    public override void Cancel()
    {
        Status = BehaviorState.Inactive;
        stateMachine.Stop();
        renderer.material = originalMaterial;
        if (Eaten)
            Destroy(gameObject);
    }

    public override void Use(PetAgent agent)
    {
        Status = BehaviorState.Ongoing;
        stateMachine.Start(agent, this);
        renderer.material = highlightMaterial;
    }

    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        originalMaterial = renderer.material;
        
        var moving = new PursueState();
        var eating = new EatingTreatState(timeToChew);
        var interactionCondition = new InteractionCondition(interaction);

        var toEating = new Transition(eating, interactionCondition);
        var toExit = new Transition(PetState.ExitState, eating.TimerCondition);

        moving.Transitions.Add(toEating);
        eating.Transitions.Add(toExit);
        stateMachine = new PetStateMachine(moving);
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        stateMachine.Update();
    }
}

public class EatingTreatState : PetState
{
    float timeToChew;

    // Use this to transition out of the state at the correct time.
    public TimerCondition TimerCondition { get; private set; } = new TimerCondition();

    public EatingTreatState(float timeToChew)
    {
        this.timeToChew = timeToChew;
    }

    public override void OnEntry(PetAgent agent, Behavior behavior)
    {
        agent.Motor.Stop();
        agent.Motor.StartWaggingTail();
        agent.Mouth.Carry(behavior);
        agent.Mjam();
        TimerCondition.StopTime = timeToChew + Time.time;
        if (agent.InternalModel.Contains(InternalState.InBed))
            agent.Learning.StartLearning("Bed");
    }

    public override void OnExit(PetAgent agent, Behavior behavior)
    {
        EatTreat eat = behavior as EatTreat;
        agent.HungerController.ChangeFill(eat.FillValue);
        eat.Eaten = true;
        agent.Mouth.Release();
        agent.Learning.StopLearning("Bed");
        agent.Motor.StopWaggingTail();
    }
}
