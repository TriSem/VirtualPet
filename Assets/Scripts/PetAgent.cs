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
}
