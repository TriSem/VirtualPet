using System.Collections.Generic;
using UnityEngine;

public abstract class ActionObject : MonoBehaviour, IBehaviour
{
    // Predicts how the pets drives will change when using this action.
    [SerializeField] 
    protected List<Outcome> outcomes = null;
    
    [SerializeField] 
    protected float priorityBonus = 0f;

    // A bonus to the actions utility. Can be used to
    // simulate reflexes.
    public float PriorityBonus => priorityBonus;

    public ActionStatus Status { get; protected set; } = ActionStatus.Inactive;

    public virtual bool PreconditionsMet(InternalModel internalModel) => true;

    public  float CalculateUtility(DriveVector drives) => drives.CalculateUtility(outcomes) + priorityBonus;

    public abstract void Use(PetAgent agent);

    public abstract void Cancel();
}

public enum ActionStatus
{
    Inactive,
    Ongoing,
    Completed
}

public interface IPhysicsObject
{
    Rigidbody Rigidbody { get; }
    Collider Collider { get; }
}

public abstract class IntermediaryAction : ActionObject
{
    protected ActionObject followUp;

    public abstract HashSet<InternalState> GetPredictedChanges();

    public void SetFollowUp(ActionObject followUp) => this.followUp = followUp;

    protected void Transition(PetAgent agent)
    {
        Status = ActionStatus.Completed;
        if (followUp == null)
            return;

        if (followUp.PreconditionsMet(agent.InternalModel))
            followUp.Use(agent);
        followUp = null;
    }
}

public class ActionSequence
{
    PetAgent agent;
    ActionObject[] actions;
    int currentStage = 0;

    public int Length => actions.Length;

    public ActionStatus Status { get; private set; } = ActionStatus.Inactive;

    public ActionSequence(ActionObject[] actions)
    {
        this.actions = actions;
    }

    public void Start(PetAgent agent)
    {
        Status = ActionStatus.Ongoing;
        this.agent = agent;
        currentStage = 0;
        actions[0].Use(agent);
    }

    public void Update()
    {
        if (Status == ActionStatus.Inactive || Status == ActionStatus.Completed)
            return;

        if(actions[currentStage].Status == ActionStatus.Completed)
        {
            if(currentStage < Length - 1)
            {
                actions[currentStage++].Cancel();
                if (actions[currentStage].PreconditionsMet(agent.InternalModel))
                    actions[currentStage].Use(agent);
                else
                    Status = ActionStatus.Completed;
            }
            else
            {
                Status = ActionStatus.Completed;
            }
        }
    }

    bool IsInSequence(ActionObject actionObject)
    {
        foreach(var action in actions)
        {
            if (action == actionObject)
                return true;
        }
        return false;
    }
}