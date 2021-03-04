public class Follow : ActionObject
{
    PetAgent agent = null;

    public override void Cancel()
    {
        Status = ActionStatus.Inactive;
    }

    public override void Use(PetAgent agent)
    {
        this.agent= agent;
        Status = ActionStatus.Ongoing;
    }

    void Update()
    {
        if (Status == ActionStatus.Ongoing)
        {
        }
    }
}
