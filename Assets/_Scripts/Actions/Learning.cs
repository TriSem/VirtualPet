using System.Collections.Generic;
using UnityEngine;

public class Learning : MonoBehaviour
{
    [Tooltip("Actions that can be associated with a command.")]
    [SerializeField] List<string> actionNames = null;
    Dictionary<string, >
    bool learning = false;
    string currentlyLearning = "";

    void Start()
    {
        foreach(var name in actionNames)
        {
            phrases.Add(new Phrase(name));
        }
    }

    public void PhraseHeard(string phrase)
    {
        if (learning)
        {

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

public class Phrase
{
    string name;
    Dictionary<string, int> actionToCount;

    public Phrase(string name)
    {
        this.name = name;
    }

    public string LikeliestAction()
    {
        float total = TotalTimesRegistered();
        float highestMatch = 0f;
        foreach(var entry in actionToCount)
        {
            float likelihood = (float)entry.Value / total;
        }
        return null;
    }

    public int TotalTimesRegistered()
    {
        int result = 0;
        foreach (var element in actionToCount)
            result += element.Value;
        return result;
    }
}