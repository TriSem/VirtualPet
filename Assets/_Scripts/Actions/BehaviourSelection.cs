using System.Collections.Generic;
using UnityEngine;

public class BehaviourSelection : MonoBehaviour
{
    [SerializeField] PetAgent agent = default;
    [SerializeField] Transform internalActionSet = default;
    [SerializeField] IntermediaryAction fallbackBehavior = default;
    [SerializeField] float commandBonus;

    HashSet<string> commandedActions = new HashSet<string>();

    List<IntermediaryAction> internalActions = default;
    public Option CurrentOption { get; private set; } = default;

    void Start()
    {
        internalActions = new List<IntermediaryAction>(internalActionSet.GetComponents<IntermediaryAction>());
        CurrentOption = new Option(fallbackBehavior);
        CurrentOption.Use(agent);
    }

    public void EvaluateActions()
    {
        var model = new InternalModel(agent.InternalModel);

        var percievedObjects = agent.Perception.Poll();
        var allActions = new List<ActionObject>(internalActions);
        foreach (var percieved in percievedObjects)
            allActions.AddRange(percieved.Actions);

        var possibleActions = new List<ActionObject>();
        var impossibleActions = new List<ActionObject>();

        List<Option> options = new List<Option>();

        foreach(var action in allActions)
        {
            if (action.PreconditionsMet(model))
                possibleActions.Add(action);
            else
                impossibleActions.Add(action);
        }

        // Add all action sequences
        foreach(var action in possibleActions)
        {
            // Even intermediaries can be chosen alone.
            var option = new Option(action);
            option.TotalUtility = TotalUtility(option);
            options.Add(option);

            if(action is IntermediaryAction intermediary)
            {
                // Develop two-step plan.
                var predictedChanges = intermediary.GetPredictedChanges();
                model.Add(predictedChanges);
                var enabledActions = EnabledActions(impossibleActions, model);
                model.Remove(predictedChanges);

                foreach(var enabledAction in enabledActions)
                {
                    option = new Option(enabledAction, intermediary);
                    option.TotalUtility = TotalUtility(option);
                    options.Add(option);
                }
            }
        }

        if(options.Count > 0)
        {
            options.Sort((x, y) => y.TotalUtility.CompareTo(x.TotalUtility));
            float highestUtility = options[0].TotalUtility;
            var actionStatus = CurrentOption.GetStatus();
            if (actionStatus == ActionStatus.Ongoing && highestUtility <= CurrentOption.TotalUtility)
            {
                return;
            }

            if (actionStatus == ActionStatus.Inactive || actionStatus == ActionStatus.Completed)
            {
                // Select several reasonable options to randomly choose from.
                CullBelowUtility(highestUtility * 0.9f, options);
            }
            else
            {
                // Ongoing abilities should only be interrupted if they are suboptimal.
                CullBelowUtility(CurrentOption.TotalUtility, options);
            }

            int index = Random.Range(0, options.Count - 1);
            CurrentOption = options[index];
            CurrentOption.Use(agent);
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

    public float TotalUtility(Option option)
    {
        float utility = option.Main.PriorityBonus + option.Main.CalculateUtility(agent.DriveVector);
        utility += (option.Intermediary?.CalculateUtility(agent.DriveVector) + option.Intermediary?.PriorityBonus) ?? 0f;

        // Grant a bonus if the action was commanded by the player.
        string actionName = option.Main.GetType().Name;
        utility += commandedActions.Contains(actionName) ? commandBonus : 0f;

        return utility;
    }

    public void CullBelowUtility(float utilityThreshold, List<Option> options)
    {
        for (int i = options.Count - 1; i > 0; i--)
        {
            if (options[i].TotalUtility < utilityThreshold)
                options.RemoveAt(i);
        }
    }

    // The named action will start recieving a bonus when calculating utility.
    public void StartCommanding(string actionName)
    {
        commandedActions.Add(actionName);
    }
    
    public void StopCommanding(string actionName)
    {
        commandedActions.Remove(actionName);
    }
}

public class Option
{
    public IntermediaryAction Intermediary{ get; private set; } = null;
    public ActionObject Main { get; private set; }
    public float TotalUtility { get; set; } = 0;

    public Option(ActionObject main, IntermediaryAction intermediary)
    {
        Intermediary = intermediary;
        Main = main;
    }

    public Option(ActionObject action)
    {
        Main = action;
    }

    public void Use(PetAgent agent)
    {
        if (Intermediary != null)
        {
            Intermediary.SetFollowUp(Main);
            Intermediary.Use(agent);
        }
        else
            Main.Use(agent);
    }

    public ActionStatus GetStatus()
    {
        var intermediaryStatus = Intermediary?.Status ?? ActionStatus.Completed;
        if (intermediaryStatus == ActionStatus.Ongoing || Main.Status == ActionStatus.Ongoing)
            return ActionStatus.Ongoing;
        else if (intermediaryStatus == ActionStatus.Completed && Main.Status == ActionStatus.Completed)
            return ActionStatus.Completed;
        else
            return ActionStatus.Inactive;
    }

    public ActionObject GetCurrentAction()
    {
        if (Main.Status == ActionStatus.Ongoing || 
            Main.Status == ActionStatus.Completed || 
            Intermediary == null)
            return Main;
        return Intermediary;
    }
}
