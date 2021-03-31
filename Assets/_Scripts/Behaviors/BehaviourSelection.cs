using System;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourSelection : MonoBehaviour
{
    [SerializeField] PetAgent agent = default;
    [SerializeField] Transform internalBehaviorSet = default;
    [SerializeField] Behavior fallbackBehavior = default;
    [SerializeField] float commandBonus;

    HashSet<string> commandedBehaviors = new HashSet<string>();
    Dictionary<string, float> behaviorBoni = new Dictionary<string, float>();

    List<Behavior> internalBehaviors = default;
    public Behavior CurrentBehavior { get; private set; } = default;
    public float CurrentUtility { get; private set; } = default;

    void Start()
    {
        internalBehaviors = new List<Behavior>(internalBehaviorSet.GetComponents<Behavior>());
        CurrentBehavior = fallbackBehavior;
        CurrentBehavior.Use(agent);
    }

    public void EvaluateBehaviors()
    {
        var model = new InternalModel(agent.InternalModel);
        List<Tuple<Behavior, float>> candidates = new List<Tuple<Behavior, float>>();

        var percievedObjects = agent.Perception.Poll();
        var allBehaviors = new List<Behavior>(internalBehaviors);
        foreach (var percieved in percievedObjects)
            allBehaviors.AddRange(percieved.Behaviors);

        var possibleBehaviors = new List<Behavior>();
        var impossibleBehaviors = new List<Behavior>();

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
            var candidate = behavior;
            candidates.Add(new Tuple<Behavior, float>(candidate, TotalUtility(candidate)));

            if(behavior is IIntermediary intermediary)
            {
                // Develop two-step plan.
                var predictedChanges = intermediary.GetPredictedChanges();
                model.Add(predictedChanges);
                var enabledBehaviors = EnabledBehaviors(impossibleBehaviors, model);
                model.Remove(predictedChanges);

                foreach(var enabledBehavior in enabledBehaviors)
                {
                    candidates.Add(new Tuple<Behavior, float>(behavior, TotalUtility(enabledBehavior, behavior)));
                }
            }
        }

        if(candidates.Count > 0)
        {
            candidates.Sort((x, y) => y.Item2.CompareTo(x.Item2));
            float highestUtility = candidates[0].Item2;
            var currentStatus = CurrentBehavior.Status;
            if (currentStatus == BehaviorState.Ongoing && highestUtility <= CurrentUtility)
            {
                return;
            }
            else
            {
                // Select several reasonable options to randomly choose from.
                CullBelowUtility(highestUtility * 0.9f, candidates);
            }

            int index = UnityEngine.Random.Range(0, candidates.Count - 1);
            CurrentBehavior = candidates[index].Item1;
            CurrentUtility = candidates[index].Item2;
            CurrentBehavior.Use(agent);
        }
    }

    List<Behavior> EnabledBehaviors(List<Behavior> previouslyImpossible, InternalModel predictedModel)
    {
        var nowPossible = new List<Behavior>();
        foreach(var behavior in previouslyImpossible)
        {
            if (behavior.PreconditionsMet(predictedModel))
                nowPossible.Add(behavior);
        }
        return nowPossible;
    }

    float TotalUtility(Behavior candidate, Behavior intermediary = null)
    {
        float utility = candidate.CalculateUtility(agent.DriveVector);

        if(intermediary != null)
            utility += intermediary.CalculateUtility(agent.DriveVector);

        // Grant a bonus if the behavior was commanded by the player.
        string behaviorName = candidate.GetType().Name;
        utility += commandedBehaviors.Contains(behaviorName) ? commandBonus : 0f;
        utility += behaviorBoni.ContainsKey(behaviorName) ? behaviorBoni[behaviorName] : 0f;

        return utility;
    }

    void CullBelowUtility(float utilityThreshold, List<Tuple<Behavior, float>> candidates)
    {
        for (int i = candidates.Count - 1; i > 0; i--)
        {
            if (candidates[i].Item2 < utilityThreshold)
                candidates.RemoveAt(i);
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

    // Give a custom bonus to the utility of the named behavior.
    public void GiveBonus(string behaviorName, float bonus)
    {
        if (!behaviorBoni.ContainsKey(behaviorName))
            behaviorBoni.Add(behaviorName, bonus);
        else
            behaviorBoni[behaviorName] = bonus;
    }

    public void RemoveBonus(string behaviorName)
    {
        behaviorBoni.Remove(behaviorName);
    }
}

//public class Option
//{
//    public IntermediaryBehavior Intermediary{ get; private set; } = null;
//    public Behavior Main { get; private set; }
//    public float TotalUtility { get; set; } = 0;

//    public Option(Behavior main, IntermediaryBehavior intermediary)
//    {
//        Intermediary = intermediary;
//        Main = main;
//    }

//    public Option(Behavior behavior)
//    {
//        Main = behavior;
//    }

//    public void Use(PetAgent agent)
//    {
//        if (Intermediary != null)
//        {
//            Intermediary.SetFollowUp(Main);
//            Intermediary.Use(agent);
//        }
//        else
//            Main.Use(agent);
//    }

//    public BehaviorState GetStatus()
//    {
//        var intermediaryStatus = Intermediary?.Status ?? BehaviorState.Completed;
//        if (intermediaryStatus == BehaviorState.Ongoing || Main.Status == BehaviorState.Ongoing)
//            return BehaviorState.Ongoing;
//        else if (intermediaryStatus == BehaviorState.Completed && Main.Status == BehaviorState.Completed)
//            return BehaviorState.Completed;
//        else
//            return BehaviorState.Inactive;
//    }

//    public Behavior GetCurrentBehavior()
//    {
//        if (Main.Status == BehaviorState.Ongoing || 
//            Main.Status == BehaviorState.Completed || 
//            Intermediary == null)
//            return Main;
//        return Intermediary;
//    }
//}
