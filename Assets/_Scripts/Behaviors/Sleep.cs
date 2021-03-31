public class Sleep : Behavior
{
    PetAgent agent = null;

    public override void Cancel()
    {
        agent.Motor.GetUp();
        agent.InternalModel.Remove(InternalState.Sleeping);
        agent.InternalModel.Remove(InternalState.LyingDown);
        Status = BehaviorState.Inactive;
    }

    public override bool PreconditionsMet(InternalModel internalModel)
    {
        return internalModel.Contains(InternalState.InBed);
    }

    public override void Use(PetAgent agent)
    {
        Status = BehaviorState.Ongoing;
        this.agent = agent;
        agent.Motor.LieDown();
        agent.InternalModel.Add(InternalState.Sleeping);
        agent.InternalModel.Add(InternalState.LyingDown);
    }
}
