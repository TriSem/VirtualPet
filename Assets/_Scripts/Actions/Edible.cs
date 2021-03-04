using UnityEngine;

public class Edible : ActionObject
{
    PetAgent agent = null;

    [SerializeField, Range(0f, 1000f)] 
    float fillValue = 5f;

    [SerializeField] bool refillable = false;
    [SerializeField] Interaction interaction = default;

    public override void Cancel()
    {
        Status = ActionStatus.Inactive;
    }

    public override void Use(PetAgent agent)
    {
        this.agent = agent;
        Status = ActionStatus.Ongoing;
    }

    void GetEaten()
    {
        agent.Stomach.ChangeFill(fillValue);
        Cancel();
        agent.Motor.Stop();
        Destroy(gameObject);
    }

    void Update()
    {
        if(Status == ActionStatus.Ongoing)
        {
            if (interaction.PetInRange)
                GetEaten();
        }
    }
}
