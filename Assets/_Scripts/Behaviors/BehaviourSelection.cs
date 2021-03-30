using System.Collections.Generic;
using UnityEngine;

public class BehaviourSelection : MonoBehaviour
{
    [SerializeField] PetAgent agent = default;
    [SerializeField] Transform internalBehaviorSet = default;
    [SerializeField] Behavior fallbackBehavior = default;
    [SerializeField] float commandBonus;

    HashSet<string> commandedBehaviors = new HashSet<string>();

    List<Behavior> internalBehaviors = default;
    public Option CurrentOption { get; private set; } = default;

    void Start()
    {
        internalBehaviors = new List<Behavior>(internalBehaviorSet.GetComponents<Behavior>());
        CurrentOption = new Option(fallbackBehavior);
        CurrentOption.Use(agent);
    }

    public void EvaluateBehaviors()
    {
        var model = new InternalModel(agent.InternalModel);

        var percievedObjects = agent.Perception.Poll();
        var allBehaviors = new List<Behavior>(internalBehaviors);
        foreach (var percieved in percievedObjects)
            allBehaviors.AddRange(percieved.Behaviors);

        var possibleBehaviors = new List<Behavior>();
        var impossibleBehaviors = new List<Behavior>();

        List<Option> options = new List<Option>();

        foreach(var behavior in allBehaviors)
        {
            if (behavior.PreconditionsMet(model))
                possibleBehaviors.Add(behavior);
            else
                impossibleBehaviors.Add(behavior);
        }

        // Add all behavior sequences
        foreach(var behavior in possibleBehaviors)
        {
            // Even intermediaries can be chosen alone.
            var option = new Option(behavior);
            option.TotalUtility = TotalUtility(option);
            options.Add(option);

            if(behavior is IntermediaryBehavior intermediary)
            {
                // Develop two-step plan.
                var predictedChanges = intermediary.GetPredictedChanges();
                model.Add(predictedChanges);
                var enabledBehaviors = EnabledBehaviors(impossibleBehaviors, model);
                model.Remove(predictedChanges);

                foreach(var enabledBehavior in enabledBehaviors)
                {
                    option = new Option(enabledBehavior, intermediary);
                    option.TotalUtility = TotalUtility(option);
                    options.Add(option);
                }
            }
        }

        if(options.Count > 0)
        {
            options.Sort((x, y) => y.TotalUtility.CompareTo(x.TotalUtility));
            float highestUtility = options[0].TotalUtility;
            var behaviorStatus = CurrentOption.GetStatus();
            if (behaviorStatus == BehaviorState.Ongoing && highestUtility <= CurrentOption.TotalUtility)
            {
                return;
            }

            if (behaviorStatus == BehaviorState.Inactive || behaviorStatus == BehaviorState.Completed)
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

    public List<Behavior> EnabledBehaviors(List<Behavior> previouslyImpossible, InternalModel predictedModel)
    {
        var nowPossible = new List<Behavior>();
        foreach(var behavior in previouslyImpossible)
        {
            if (behavior.PreconditionsMet(predictedModel))
                nowPossible.Add(behavior);
        }
        return nowPossible;
    }

    public float TotalUtility(Option option)
    {
        float utility = option.Main.PriorityBonus + option.Main.CalculateUtility(agent.DriveVector);
        utility += (option.Intermediary?.CalculateUtility(agent.DriveVector) + option.Intermediary?.PriorityBonus) ?? 0f;

        // Grant a bonus if the behavior was commanded by the player.
        string behaviorName = option.Main.GetType().Name;
        utility += commandedBehaviors.Contains(behaviorName) ? commandBonus : 0f;

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

    // The named behavior will start recieving a bonus when calculating utility.
    public void StartCommanding(string behaviorName)
    {
        commandedBehaviors.Add(behaviorName);
    }
    
    public void StopCommanding(string behaviorName)
    {
        commandedBehaviors.Remove(behaviorName);
    }
}

public class Option
{
    public IntermediaryBehavior Intermediary{ get; private set; } = null;
    public Behavior Main { get; private set; }
    public float TotalUtility { get; set; } = 0;

    public Option(Behavior main, IntermediaryBehavior intermediary)
    {
        Intermediary = intermediary;
        Main = main;
    }

    public Option(Behavior behavior)
    {
        Main = behavior;
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

    public BehaviorState GetStatus()
    {
        var intermediaryStatus = Intermediary?.Status ?? BehaviorState.Completed;
        if (intermediaryStatus == BehaviorState.Ongoing || Main.Status == BehaviorState.Ongoing)
            return BehaviorState.Ongoing;
        else if (intermediaryStatus == BehaviorState.Completed && Main.Status == BehaviorState.Completed)
            return BehaviorState.Completed;
        else
            return BehaviorState.Inactive;
    }

    public Behavior GetCurrentBehavior()
    {
        if (Main.Status == BehaviorState.Ongoing || 
            Main.Status == BehaviorState.Completed || 
            Intermediary == null)
            return Main;
        return Intermediary;
    }
}
