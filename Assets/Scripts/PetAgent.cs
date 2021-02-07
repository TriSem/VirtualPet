using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PetAgent : MonoBehaviour
{
    [SerializeField] float tickRate = (1 / 10);
    [SerializeField] MotorSystem motorSystem = null;

    public NavMeshAgent NavAgent { get; private set; }
    public DriveVector DriveVector { get; } = new DriveVector();

    EventTree events = new EventTree();


    List<Trait> percievedTraits = new List<Trait>();

    public List<T> GetTraits<T>() where T : Trait
    {
        var selected = new List<T>();
        foreach(var trait in percievedTraits)
        {
            if (trait is T t)
                selected.Add(t);
        }

        return selected;
    }

    float lastTick = (1 / 10);


    void Awake()
    {
        NavAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float time = Time.time;
        if (lastTick + tickRate < time)
        {
            lastTick = time;
        }
    }

    public bool EventActive(EventQuery query)
    {
        return events.EventActive(query);
    }
}
