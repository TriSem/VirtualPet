using System;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourSelection : MonoBehaviour
{
    [SerializeField] PetAgent agent = default;
    [SerializeField] Transform reactionSet = default;
    [SerializeField] ActionObject fallbackBehavior = default;
    [SerializeField] float priorityBonus;

    HashSet<string> reinforcedActions;

    List<Reaction> reactions = default;
    public IBehaviour CurrentAction { get; private set; } = default;
    public float CurrentUtility { get; private set; } = 0f;

    void Start()
    {
        reactions = new List<Reaction>(reactionSet.GetComponents<Reaction>());
        CurrentAction = fallbackBehavior;
        CurrentAction.Use(agent);
    }

    public void EvaluateActions()
    {
        foreach (var reaction in reactions)
        {
            if (reaction.Triggered)
            {
                CurrentAction.Cancel();
                CurrentAction = reaction;
                CurrentAction.Use(agent);
                CurrentUtility = float.MaxValue;
                return;
            }
        }

        var worldObjects = agent.Perception.GetWorldObjects();
        var actions = new List<ActionObject>();
        foreach (var worldObject in worldObjects)
        {
            actions.AddRange(worldObject.Actions);
        }

        var actionUtilities = new List<Tuple<float, ActionObject>>();
        foreach (var action in actions)
        {
            if(action.IsUsable())
            {
                float utility = action.CalculateUtility(agent.DriveVector);
                if (reinforcedActions.Contains(action.GetType().Name))
                    utility += priorityBonus;
                actionUtilities.Add(new Tuple<float, ActionObject>(utility, action));
            }
        }

        if(actionUtilities.Count > 0)
        {
            actionUtilities.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            float highestUtility = actionUtilities[0].Item1;
            if(CurrentAction.Status == ActionStatus.Ongoing && highestUtility <= CurrentUtility)
            {
                return;
            }

            if(CurrentAction.Status == ActionStatus.Inactive)
            {
                CullBelowUtility(highestUtility * 0.9f, actionUtilities);
            }
            else
            {
                CullBelowUtility(CurrentUtility, actionUtilities);
            }

            int index = UnityEngine.Random.Range(0, actionUtilities.Count - 1);
            CurrentUtility = actionUtilities[index].Item1;
            CurrentAction = actionUtilities[index].Item2;
            CurrentAction.Use(agent);
        }
    }

    public IBehaviour CheckForReactions()
    {
        foreach(var reaction in reactions)
        {
            if (reaction.Triggered)
                return reaction;
        }
        return null;
    }

    public void CullBelowUtility(float utilityThreshold, List<Tuple<float, ActionObject>> actionUtility)
    {
        for (int i = actionUtility.Count - 1; i > 0; i--)
        {
            if (actionUtility[i].Item1 < utilityThreshold)
                actionUtility.RemoveAt(i);
        }
    }

    public void ReinforceAction(string actionName)
    {
        reinforcedActions.Add(actionName);
    }
    
    public void EndReinforcement(string actionName)
    {
        reinforcedActions.Remove(actionName);
    }
}
