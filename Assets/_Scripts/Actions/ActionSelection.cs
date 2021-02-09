using System.Collections.Generic;
using UnityEngine;

public class ActionSelection : MonoBehaviour
{
    [SerializeField] PetAgent agent = default;
    [SerializeField] Transform reactionSet = default;
    [SerializeField] ActionObject fallbackBehavior = default;

    List<Reaction> reactions = default;
    public IAction CurrentAction { get; private set; } = default;
    public float CurrentUtility { get; private set; } = 0f;

    void Start()
    {
        reactions = new List<Reaction>(reactionSet.GetComponents<Reaction>());
    }

    public void SelectAction()
    {
        var reaction = CheckForReactions();
        if (
            reaction != null &&
            !(CurrentAction is Reaction))
        {
            CurrentAction.Cancel();
            CurrentAction = reaction;
            CurrentAction.UseAction(agent);
            CurrentUtility = float.MaxValue;
        }
        else if (CurrentAction == null)
        {
            CurrentAction = fallbackBehavior;
            CurrentUtility = fallbackBehavior.CalculateUtility(agent.DriveVector);
            CurrentAction.UseAction(agent);
        }
        else if (CurrentAction.Status != ActionStatus.Ongoing)
        {
            HashSet<WorldObject> percievedObjects = agent.Perception.GetWorldObjects();
            ChooseNewAction(percievedObjects);
            CurrentAction.UseAction(agent);
        }
    }

    public IAction CheckForReactions()
    {
        foreach(var reaction in reactions)
        {
            if (reaction.Triggered)
                return reaction;
        }
        return null;
    }

    public void ChooseNewAction(HashSet<WorldObject> worldObjects)
    {
        ActionObject mostUseful = fallbackBehavior;
        float highestUtility = float.MinValue;

        foreach (var worldObject in worldObjects)
        {
            foreach (var action in worldObject.Actions)
            {
                if (action.IsUsable())
                {
                    float utility = action.CalculateUtility(agent.DriveVector);
                    if (utility > highestUtility)
                    {
                        mostUseful = action;
                        highestUtility = utility;
                    }
                }
            }
        }

        CurrentUtility = highestUtility;
        CurrentAction = mostUseful;
    }
}
