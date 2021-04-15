using UnityEngine;

public class Fetch : Behavior
{
    [SerializeField, Tooltip("How far from the goal should the pet drop the object off?")] 
    float deliveryDistance = 1.5f;

    [SerializeField, Tooltip("How long should the pet wait after delivering?")] 
    float waitForRewardTime = 4f;
    PetStateMachine stateMachine = null;
    

    public override void Cancel()
    {
        stateMachine.Stop();
        Status = BehaviorState.Inactive;
    }

    public override void Use(PetAgent agent)
    {
        Status = BehaviorState.Ongoing;

        var distanceCondition = new WithinDistanceCondition(agent.transform, transform, deliveryDistance);
        var follow = new FollowState();
        var drop = new DropState();
        var awaitReward = new AwaitRewardState(waitForRewardTime);
        var exit = new ExitState();

        var exitCondition = new OrCondition(awaitReward.TimerCondition, new InternalStateCondition(agent.InternalModel, InternalState.BeingPet));

        var toDrop = new Transition(drop, distanceCondition);
        var toAwait = new Transition(awaitReward, new MetCondition());
        var toExit = new Transition(exit, exitCondition);

        follow.Transitions.Add(toDrop);
        drop.Transitions.Add(toAwait);
        awaitReward.Transitions.Add(toExit);

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

public class AwaitRewardState : PetState
{
    public TimerCondition TimerCondition { get; } = new TimerCondition();
    float waitTime;

    public AwaitRewardState(float waitTime)
    {
        this.waitTime = waitTime;
    }

    public override void OnEntry(PetAgent agent, Behavior behavior)
    {
        TimerCondition.StopTime = Time.time + waitTime;
        agent.Learning.StartLearning("Fetch");
        agent.Motor.Stop();
        agent.Motor.StartWaggingTail();
    }

    public override void OnExit(PetAgent agent, Behavior behavior)
    {
        agent.Learning.StopLearning("Fetch");
        agent.Motor.StopWaggingTail();
    }
}
