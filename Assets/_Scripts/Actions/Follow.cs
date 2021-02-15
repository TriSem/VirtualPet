public class Follow : ActionObject
{
    PetAgent agent = null;

    public override void Cancel()
    {
        Status = ActionStatus.Inactive;
    }

    public override void UseAction(PetAgent agent)
    {
        this.agent= agent;
        Status = ActionStatus.Ongoing;
    }

    void Update()
    {
        if (Status == ActionStatus.Ongoing)
        {
            agent.Motor.MoveTo(transform.position);
            if (agent.Motor.Arrived)
                Cancel();
        }
    }
}
