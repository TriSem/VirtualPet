using UnityEngine;

public class EnjoyAffection : Behavior
{
    PetAgent agent;
    [SerializeField] Transform particles = null;
    [SerializeField] float noTouchTimer = 5f;

    float timer = 0f;
    bool timerOn = false;

    public override void Cancel()
    {
        Status = BehaviorState.Inactive;
        agent.Motor.GetUp();
        agent.Motor.StopWaggingTail();
        particles.gameObject.SetActive(false);
    }

    public override void Use(PetAgent agent)
    {
        this.agent = agent;
        Status = BehaviorState.Ongoing;
        particles.gameObject.SetActive(true);
        agent.Motor.SitDown();
        agent.Motor.StartWaggingTail();
    }

    public override bool PreconditionsMet(InternalModel internalModel)
    {
        return internalModel.Contains(InternalState.BeingPet);
    }

    void Start()
    {
        particles.gameObject.SetActive(false);    
    }

    void Update()
    {
        if (Status == BehaviorState.Ongoing && !timerOn && !agent.InternalModel.Contains(InternalState.BeingPet))
        {
            timerOn = true;
            timer = noTouchTimer;
            particles.gameObject.SetActive(false);
        }
        if(timerOn)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Cancel();
                timerOn = false;
            }
        }
    }
}
