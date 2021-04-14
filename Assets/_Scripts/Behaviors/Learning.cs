using System;
using System.Collections.Generic;
using UnityEngine;

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

    Material originalMaterial;
    MeshRenderer iconRenderer;
 
    Dictionary<string, LearnableBehavior> learnableBehaviors = new Dictionary<string, LearnableBehavior>();
    List<Tuple<LearnableBehavior, float>> recentlyHeard = new List<Tuple<LearnableBehavior, float>>();
    LearnableBehavior currentlyLearning = null;
    float nextLearningAvailable = 0f;

    void Awake()
    {
        foreach(var learnable in learnables)
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
        // Remove priority if command was issued too long ago.
        foreach (var tuple in recentlyHeard)
        {
            if (Time.time >= tuple.Item2)
                behaviourSelection.StopCommanding(tuple.Item1.Name);
        }
    }

    public void PhraseHeard(string phrase)
    {
        Debug.Log("Heard: " + phrase);
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
                    Debug.Log("Start Commanding: " + entry.Value.Name);
                }
            }
        }
    }

    public void StartLearning(string behaviorName)
    {
        if (nextLearningAvailable >= Time.time) ;
        {
            Debug.Log("Start learning: " + behaviorName);
            icon.gameObject.SetActive(true);
            currentlyLearning = learnableBehaviors[behaviorName];
            nextLearningAvailable = Time.time + learningCooldown;
        }
        Debug.Log("Learning not available");
    }

    public void StopLearning(string behaviorName)
    {
        if(currentlyLearning != null && currentlyLearning.Name == behaviorName)
        {
            Debug.Log("Stop learning: " + behaviorName);
            icon.gameObject.SetActive(false);
            currentlyLearning = null;
            iconRenderer.material = originalMaterial;
        }
    }

    public void RecieveCommand(string command)
    {
        PhraseHeard(command);
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