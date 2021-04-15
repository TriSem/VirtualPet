using System;
using System.Collections.Generic;
using UnityEngine;

/* Enables the pet to associate heard phrases with behaviors.
 * Learned behaviors will get a bonus to their utility evaluation
 * when the corresponding command is heard.
 */
public class Learning : MonoBehaviour, ICommandReceiver
{
    [SerializeField, Tooltip("Actions that can be associated with a command.")]
    List<LearnableBehavior> learnables = null;

    [SerializeField]
    BehaviourSelection behaviourSelection = null;

    [SerializeField, Tooltip("This will signal that learning is activated.")]
    Transform icon = null;

    [SerializeField]
    CommandHub commandHub = null;

    [SerializeField, Tooltip("Determines how many seconds the command bonus will last.")]
    float commandDuration = 3;

    [SerializeField]
    float learningCooldown = 5f;

    [SerializeField]
    Material commandHeardMaterial = null;

    readonly Dictionary<string, LearnableBehavior> learnableBehaviors = new Dictionary<string, LearnableBehavior>();
    readonly List<Tuple<LearnableBehavior, float>> recentlyHeard = new List<Tuple<LearnableBehavior, float>>();
    Material originalMaterial;
    MeshRenderer iconRenderer;
    LearnableBehavior currentlyLearning = null;
    float nextLearningAvailable = 0f;

    void Awake()
    {
        foreach (var learnable in learnables)
        {
            learnableBehaviors.Add(learnable.Name, learnable);
        }

        commandHub.Register(this);
    }

    void Start()
    {
        iconRenderer = icon.GetComponent<MeshRenderer>();
        originalMaterial = iconRenderer.material;
    }

    void Update()
    {
        float time = Time.time;

        // Remove priority if command was issued too long ago.
        for (int i = recentlyHeard.Count - 1; i >= 0; i--)
        {
            var tuple = recentlyHeard[i];
            if (time >= tuple.Item2)
            {
                behaviourSelection.StopCommanding(tuple.Item1.Name);
                recentlyHeard.RemoveAt(i);
            }
        }
    }

    public void PhraseHeard(string phrase)
    {
        // If a command can be learned, add the phrase to its statistic.
        if (currentlyLearning != null)
        {
            var action = learnableBehaviors[currentlyLearning.Name];
            iconRenderer.material = commandHeardMaterial;

            action.PhraseRecognized(phrase);
        }
        else
        {
            // If nothing is currently being learned the pet will count the phrase
            // as a command.

            foreach (var entry in learnableBehaviors)
            {
                if (entry.Value.AssociatedPhrase == phrase)
                {
                    recentlyHeard.Add(new Tuple<LearnableBehavior, float>(entry.Value, Time.time + commandDuration));
                    behaviourSelection.StartCommanding(entry.Value.Name);
                }
            }
        }
    }

    public void StartLearning(string behaviorName)
    {
        if (Time.time > nextLearningAvailable)
        {
            icon.gameObject.SetActive(true);
            currentlyLearning = learnableBehaviors[behaviorName];
            nextLearningAvailable = Time.time + learningCooldown;
        }
    }

    public void StopLearning(string behaviorName)
    {
        if (currentlyLearning != null && currentlyLearning.Name == behaviorName)
        {
            icon.gameObject.SetActive(false);
            currentlyLearning = null;
            iconRenderer.material = originalMaterial;
        }
    }

    public void RecieveCommand(string command)
    {
        PhraseHeard(command);
    }

    public bool HasHeardCommand(string commandName)
    {
        foreach(var tuple in recentlyHeard)
        {
            if (tuple.Item1.Name == commandName)
                return true;
        }

        return false;
    }
}

[Serializable]
public class LearnableBehavior
{
    const string None = "None";

    [SerializeField] 
    string behaviorName = None;

    [SerializeField, Tooltip("The minimum number of times the command has to be practiced.")] 
    int minSampleSize = 5;

    [SerializeField, Range(0f, 1f)] 
    [Tooltip("The percentage of the sample size needed for a phrase to be associated with a command.")] 
    float minAccuracy = 0.75f;

    int sampleSize = 0;

    public string AssociatedPhrase { get; private set; } = None;

    public string Name => behaviorName;
    Dictionary<string, int> phraseCountPairs = new Dictionary<string, int>();

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