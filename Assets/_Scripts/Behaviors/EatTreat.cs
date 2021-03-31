using UnityEngine;

public class EatTreat : Behavior
{
    [SerializeField, Range(0f, 1000f)] 
    float fillValue = 5f;

    [SerializeField] float timeToChew = 2f;

    [SerializeField] Interaction interaction = default;
    [SerializeField] Material highlightMaterial = default;

    new MeshRenderer renderer= null;
    Material originalMaterial = null;

    public bool Eaten { get; set; } = false;

    PetStateMachine stateMachine;

    public float FillValue => fillValue;

    public override void Cancel()
    {
        Status = BehaviorState.Inactive;
        stateMachine.Stop();
        if (Eaten)
            Destroy(gameObject);
        renderer.material = originalMaterial;
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
        // TODO: Start chewing animation.
        agent.Mouth.Carry(behavior);
        agent.Mjam();
        TimerCondition.StopTime = timeToChew + Time.time;
    }

    public override void OnExit(PetAgent agent, Behavior behavior)
    {
        // TODO: Stop chewing animation.
        EatTreat eat = behavior as EatTreat;
        agent.Stomach.ChangeFill(eat.FillValue);
        eat.Eaten = true;
        agent.Mouth.Release();
    }
}
