using System.Collections.Generic;
using UnityEngine;

public class Learning : MonoBehaviour
{
    [Tooltip("Actions that can be associated with a command.")]
    [SerializeField] List<string> actionNames = null;
    Dictionary<string, LearnedAction> learnedActions = new Dictionary<string, LearnedAction>();
    LearnedAction currentlyLearning = null;

    void Start()
    {
        foreach(var name in actionNames)
        {
            learnedActions.Add(name, new LearnedAction(name));
        }
    }

    public void PhraseHeard(string phrase)
    {
        if (currentlyLearning != null)
        {
            var action = learnedActions[currentlyLearning.ActionName];
            action.PhraseRecognized(phrase);
        }
        else
        {
            NotifyActionSelection();
        }
    }

    void NotifyActionSelection()
    {

    }
}

public class LearnedAction
{
    const string None = "None";
    int minSampleSize = 5;
    int sampleSize = 0;
    float minAccuracy = 0.75f;

    public string AssociatedPhrase { get; private set; } = None;

    public string ActionName { get; private set; }
    Dictionary<string, int> phraseCountPairs;

    public LearnedAction(string actionName)
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