using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(BehaviourSelection))]
public class PetAgent : MonoBehaviour
{
    [SerializeField] float tickRate = 0.5f;
    [SerializeField] MotorSystem motorSystem = null;
    [SerializeField] Perception perception = null;
    [SerializeField] PetGrasp mouth = null;
    [SerializeField] Learning learning = null;
    [SerializeField] HungerController hungerController;
    [SerializeField] Touch touch = null;
    AudioSource audioSource = null;

    public BehaviourSelection BehaviourSelection { get; private set; } = null;

    public InternalModel InternalModel { get; private set; } = new InternalModel();
    public Perception Perception => perception;
    public DriveVector DriveVector { get; } = new DriveVector();
    public MotorSystem Motor => motorSystem;
    public PetGrasp Mouth => mouth;
    public Learning Learning => learning;
    public HungerController HungerController => hungerController;

    public void Mjam()
    {
        audioSource.Play();
    }

    float lastTick = 0;

    void Awake()
    {
        BehaviourSelection = GetComponent<BehaviourSelection>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float time = Time.time;
        if (lastTick + tickRate < time)
        {
            lastTick = time;
            BehaviourSelection.EvaluateBehaviors();
        }
    }
}

public class InternalModel
{
    HashSet<InternalState> internalStates = new HashSet<InternalState>();

    public void Add(InternalState internalState) => internalStates.Add(internalState);

    public void Add(HashSet<InternalState> internalStates) => internalStates.UnionWith(internalStates);

    public void Remove(InternalState internalState) => internalStates.Remove(internalState);

    public void Remove(HashSet<InternalState> internalStates) => internalStates.IntersectWith(internalStates);

    public bool Contains(InternalState internalState) => internalStates.Contains(internalState);

    public bool ContainsAll(HashSet<InternalState> other) => other.IsSubsetOf(internalStates);

    public bool ContainsNone(HashSet<InternalState> other) => !other.Overlaps(internalStates);

    public InternalModel CopyAndAdd(HashSet<InternalState> internalStates)
    {
        var copy = new InternalModel(this);
        copy.internalStates.UnionWith(internalStates);
        return copy;
    }

    public HashSet<InternalState> ToHashSet() => new HashSet<InternalState>(internalStates);

    public InternalModel() { }

    // Deep copy constructor.
    public InternalModel(InternalModel other)
    {
        internalStates = new HashSet<InternalState>(other.internalStates);
    }
}

public enum InternalState
{
    Carrying,
    Sitting,
    LyingDown,
    BeingPet,
    InBed,
    Sleeping
}
