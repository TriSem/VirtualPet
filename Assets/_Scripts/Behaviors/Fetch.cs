using UnityEngine;

public class Fetch : Behavior
{
    [SerializeField] float deliveryDistance;
    PetStateMachine stateMachine = null;

    public override void Cancel()
    {
        stateMachine.Stop();
        Status = BehaviorState.Completed;
    }

    public override void Use(PetAgent agent)
    {
        Status = BehaviorState.Ongoing;

        var distanceCondition = new WithinDistanceCondition(agent.transform, transform, deliveryDistance);
        var follow = new FollowState();
        var drop = new DropState();
        var exit = new ExitState();

        var toDrop = new Transition(drop, distanceCondition);
        var toExit = new Transition(exit, new MetCondition());

        follow.Transitions.Add(toDrop);
        drop.Transitions.Add(toExit);

        stateMachine = new PetStateMachine(follow);

        stateMachine.Start(agent, this);
    }

    void Update()
    {
        stateMachine?.Update();
    }

    public override bool PreconditionsMet(InternalModel internalModel)
    {
        return internalModel.Contains(InternalState.Carrying);
    }
}
