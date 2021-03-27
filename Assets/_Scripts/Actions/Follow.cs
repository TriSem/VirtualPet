public class Follow : ActionObject
{
    PetAgent agent = null;

    public override void Cancel()
    {
        Status = ActionStatus.Inactive;
        agent.Motor.Stop();
    }

    public override void Use(PetAgent agent)
    {
        this.agent= agent;
        Status = ActionStatus.Ongoing;
        agent.Motor.Follow(transform);
    }

    void Update()
    {
    }
}
