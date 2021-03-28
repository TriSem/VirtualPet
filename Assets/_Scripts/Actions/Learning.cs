using System;
using System.Collections.Generic;
using UnityEngine;

public class Learning : MonoBehaviour
{
    [SerializeField, Tooltip("Actions that can be associated with a command.")]
    List<LearnableAction> learnables = null;
    [SerializeField] BehaviourSelection behaviourSelection = null;

    [SerializeField, Tooltip("Determines how many seconds the command bonus will last.")]
    float commandDuration = 3;

    Dictionary<string, LearnableAction> learnableActions = new Dictionary<string, LearnableAction>();
    List<Tuple<LearnableAction, float>> recentlyHeard = new List<Tuple<LearnableAction, float>>();
    LearnableAction currentlyLearning = null;

    void Start()
    {
        foreach(var learnable in learnables)
        {
            learnableActions.Add(name, new LearnableAction(learnable.ActionName));
        }
    }

    void Update()
    {
        // Remove priority if command was issued too long ago.
        foreach (var tuple in recentlyHeard)
        {
            if (Time.time >= tuple.Item2)
                behaviourSelection.StopCommanding(tuple.Item1.ActionName);
        }
    }

    public void PhraseHeard(string phrase)
    {
        if (currentlyLearning != null)
        {
            var action = learnableActions[currentlyLearning.ActionName];
            action.PhraseRecognized(phrase);
        }
        else
        {
            foreach (var entry in learnableActions)
            {
                if (entry.Value.AssociatedPhrase == phrase)
                {
                    recentlyHeard.Add(new Tuple<LearnableAction, float>(entry.Value, Time.time + commandDuration));
                    behaviourSelection.StartCommanding(entry.Value.ActionName);
                }
            }
        }
    }
}

[Serializable]
public class LearnableAction
{
    const string None = "None";
    [SerializeField] string actionName;
    [SerializeField] int minSampleSize = 5;
    [SerializeField] float minAccuracy = 0.75f;
    int sampleSize = 0;

    public string AssociatedPhrase { get; private set; } = None;

    public string ActionName { get; private set; }
    Dictionary<string, int> phraseCountPairs;

    public LearnableAction(string actionName)
    {
        ActionName = actionName;
        phraseCountPairs = new Dictionary<string, int>();
    }

    public void PhraseRecognized(string phrase)
    {
        if (phraseCountPairs.ContainsKey(phrase))
            phraseCountPairs[phrase]++;
        else
            phraseCountPairs.Add(phrase, 1);

        sampleSize++;
        EvaluateAssociation();
    }

    void EvaluateAssociation()
    {
        if (sampleSize < minSampleSize)
            return;

        AssociatedPhrase = None;
        foreach(var entry in phraseCountPairs)
        {
            float likeliness = (float)entry.Value / sampleSize;
            if(likeliness >= minAccuracy)
            {
                AssociatedPhrase = entry.Key;
                return;
            }    
        }
    }
}