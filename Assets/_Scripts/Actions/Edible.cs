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

    public override void UseAction(PetAgent agent)
    {
        this.agent = agent;
        agent.Motor.GoInteract(interaction);
        Status = ActionStatus.Ongoing;
    }

    void GetEaten()
    {
        agent.Stomach.ChangeFill(fillValue);
        Cancel();
        gameObject.transform.position = new Vector3(1000, 1000, 1000);
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
