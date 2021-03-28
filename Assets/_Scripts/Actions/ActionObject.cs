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

    public virtual bool Interruptible() => true;
}

public enum ActionStatus
{
    Inactive,
    Ongoing,
}

public interface IPhysicsObject
{
    Rigidbody Rigidbody { get; }
    Collider Collider { get; }
}

public interface IIntermediary
{
    HashSet<InternalState> GetPredictedChanges();
    void TransitionToNext();
}

public class ActionSequence
{
    ActionObject[] actions;
    int currentStage = 0;

    public int Length { get; private set; }

    public ActionStatus Status
    {
        get => actions[currentStage].Status == ActionStatus.Ongoing ? ActionStatus.Ongoing : ActionStatus.Inactive;
        private set => Status = value; 
    } 

    public ActionSequence(ActionObject[] actions)
    {
        this.actions = actions;
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