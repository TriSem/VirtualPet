public class Follow : Behavior
{
    PetAgent agent = null;

    public override void Cancel()
    {
        Status = BehaviorState.Inactive;
        agent.Motor.Stop();
    }

    public override void Use(PetAgent agent)
    {
        this.agent= agent;
        Status = BehaviorState.Ongoing;
        agent.Motor.Follow(transform);
    }
}
