using System;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourSelection : MonoBehaviour
{
    [SerializeField] PetAgent agent = default;
    [SerializeField] Transform internalActionSet = default;
    [SerializeField] ActionObject fallbackBehavior = default;
    [SerializeField] float commandBonus;

    HashSet<string> reinforcedActions = new HashSet<string>();

    List<ActionObject> internalActions = default;
    public IBehaviour CurrentAction { get; private set; } = default;
    public float CurrentUtility { get; private set; } = 0f;

    void Start()
    {
        internalActions = new List<ActionObject>(internalActionSet.GetComponents<ActionObject>());
        CurrentAction = fallbackBehavior;
        CurrentAction.Use(agent);
    }

    public void EvaluateActions()
    {
        ActionObject[] actionChain = new ActionObject[2];
        var model = new InternalModel(agent.InternalModel);
        var differences = new HashSet<InternalState>();

        var stimuli = agent.Perception.Poll();

        var possibleActions = new List<ActionObject>();
        var impossibleActions = new List<ActionObject>();

        foreach(var stimulus in stimuli)
        {
            var actions = stimulus.WorldObject.Actions;
            foreach(var action in actions)
            {
                if (action.PreconditionsMet(model))
                    possibleActions.Add(action);
                else
                    impossibleActions.Add(action);
            }
        }

        foreach(var action in possibleActions)
        {
            if(action is IIntermediary intermediary)
            {
                var predictedChanges = intermediary.GetPredictedChanges();
                model.Add(predictedChanges);
                var enabledActions = EnabledActions(impossibleActions, model);
                model.Remove(predictedChanges);
            }
        }
    }

    public List<ActionObject> EnabledActions(List<ActionObject> previouslyImpossible, InternalModel predictedModel)
    {
        var nowPossible = new List<ActionObject>();
        foreach(var action in previouslyImpossible)
        {
            if (action.PreconditionsMet(predictedModel))
                nowPossible.Add(action);
        }
        return nowPossible;
    }

    //public void EvaluateActions()
    //{
    //    var actionUtilities = new List<Tuple<float, IBehaviour>>();

    //    var stimuli = agent.Perception.Poll();
    //    var actions = new List<ActionObject>(internalActions);

    //    foreach (var stimulus in stimuli)
    //    {
    //        actions.AddRange(stimulus.WorldObject.Actions);
    //    }

    //    foreach (var action in actions)
    //    {
    //        if(action.PreconditionsMet(agent))
    //        {
    //            float utility = action.CalculateUtility(agent.DriveVector);
    //            utility += action.PriorityBonus;
    //            if (reinforcedActions.Contains(action.GetType().Name))
    //                utility += commandBonus;
    //            actionUtilities.Add(new Tuple<float, IBehaviour>(utility, action));
    //        }
    //    }

    //    if(actionUtilities.Count > 0)
    //    {
    //        actionUtilities.Sort((x, y) => y.Item1.CompareTo(x.Item1));
    //        float highestUtility = actionUtilities[0].Item1;
    //        if(CurrentAction.Status == ActionStatus.Ongoing && highestUtility <= CurrentUtility)
    //        {
    //            return;
    //        }

    //        if(CurrentAction.Status == ActionStatus.Inactive)
    //        {
    //            CullBelowUtility(highestUtility * 0.9f, actionUtilities);
    //        }
    //        else
    //        {
    //            CullBelowUtility(CurrentUtility, actionUtilities);
    //        }

    //        int index = UnityEngine.Random.Range(0, actionUtilities.Count - 1);
    //        CurrentUtility = actionUtilities[index].Item1;
    //        CurrentAction = actionUtilities[index].Item2;
    //        CurrentAction.Use(agent);
    //    }
    //}

    public void CullBelowUtility(float utilityThreshold, List<Tuple<float, IBehaviour>> actionUtility)
    {
        for (int i = actionUtility.Count - 1; i > 0; i--)
        {
            if (actionUtility[i].Item1 < utilityThreshold)
                actionUtility.RemoveAt(i);
        }
    }

    public void StartActionPriority(string actionName)
    {
        reinforcedActions.Add(actionName);
    }
    
    public void EndActionPriority(string actionName)
    {
        reinforcedActions.Remove(actionName);
    }
}
