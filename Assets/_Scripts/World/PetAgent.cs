﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(BehaviourSelection))]
public class PetAgent : MonoBehaviour
{
    [SerializeField] float tickRate = (1 / 10);
    [SerializeField] MotorSystem motorSystem = null;
    [SerializeField] Perception perception = null;
    [SerializeField] Stomach stomach = null;
    [SerializeField] Grasp snoot = null;
    BehaviourSelection actionSelection = null;

    public BoxCollider BoundingBox { get; private set; } = null;
    public InternalModel InternalModel { get; private set; } = new InternalModel();

    public Stomach Stomach => stomach;
    public Perception Perception => perception;
    public DriveVector DriveVector { get; } = new DriveVector();
    public MotorSystem Motor => motorSystem;
    public Grasp Snoot => snoot;

    float lastTick = (1 / 5);

    void Awake()
    {
        BoundingBox = GetComponent<BoxCollider>();
        actionSelection = GetComponent<BehaviourSelection>();
    }

    void Update()
    {
        float time = Time.time;
        if (lastTick + tickRate < time)
        {
            lastTick = time;
            actionSelection.EvaluateActions();
        }
    }
}

public class InternalModel
{
    HashSet<InternalState> internalStates = new HashSet<InternalState>();

    public void Add(InternalState internalState) => internalStates.Add(internalState);

    public void Add(HashSet<InternalState> internalStates) => internalStates.UnionWith(internalStates);

    public void Remove(InternalState internalState) => internalStates.Add(internalState);

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
    BeingPet
}
