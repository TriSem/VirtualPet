using System;
using UnityEngine;
using UnityEngine.AI;

public class MotorSystem : MonoBehaviour
{
    [SerializeField] NavMeshAgent navAgent = null;
    [SerializeField] PetAgent agent = null;
    [SerializeField] float baseSpeed = 3f;
    [SerializeField] float maximumSpeed = 6f;
    [SerializeField] float baseAngularSpeed = 135f;
    [SerializeField] float maximumAngularSpeed = 270f;
    [SerializeField] float baseStoppingDistance = 2f;

    bool stopped = true;

    SteeringBehavior currentBehavior = null;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = baseSpeed;
        navAgent.angularSpeed = baseAngularSpeed;
        navAgent.stoppingDistance = baseStoppingDistance;
    }

    void Update()
    {
        LimitCurrentSpeed();
        if(!stopped)
        {
            navAgent.destination = currentBehavior.Destination();
        }
    }

    public void RequestSpeed(float speed)
    {
        float limit = DetermineSpeedLimit();
        navAgent.speed = Mathf.Clamp(speed, 0, limit);
        AdjustAngularSpeed();
    }

    float DetermineSpeedLimit()
    {
        float limit = baseSpeed;
        return Mathf.Max(baseSpeed, limit);
    }

    /// <summary>
    /// Adjusts angular speed to use the same percentage of the maximum
    /// as regular speed.
    /// </summary>
    void AdjustAngularSpeed()
    {
        float percentage = (navAgent.speed - baseSpeed) * 100 / (maximumSpeed - baseSpeed);
        navAgent.angularSpeed = baseAngularSpeed
            + (maximumAngularSpeed - baseAngularSpeed)
            * percentage
            / 100;
    }

    void LimitCurrentSpeed()
    {
        float limit = DetermineSpeedLimit();
        navAgent.speed = Mathf.Min(navAgent.speed, limit);
        AdjustAngularSpeed();   
    }

    public void Pursue(Transform target, float pursueLead)
    {
        stopped = false;
        navAgent.updateRotation = true;
        navAgent.updatePosition = true;
        currentBehavior = new Pursuit(target, pursueLead);
    }

    public void Wander()
    {
        stopped = false;
        navAgent.updateRotation = true;
        navAgent.updatePosition = true;
        currentBehavior = new WanderSteer(navAgent.transform);
    }
    
    public void Follow(Transform target)
    {
        stopped = false;
        navAgent.updateRotation = true;
        navAgent.updatePosition = true;
        currentBehavior = new Pursuit(target, 0f);
    }

    public void Align(Transform target)
    {
        navAgent.updateRotation = true;
        navAgent.updatePosition = false;
        currentBehavior = new Pursuit(target, 0f);
    }

    public void SitDown()
    {
        stopped = true;
        agent.InternalModel.Add(InternalState.Sitting);
        // TODO: Play sit animation.
    }

    public void LieDown()
    {
        stopped = true;
        agent.InternalModel.Add(InternalState.LyingDown);
        // TODO: Play lying down animation.
    }

    public void GetUp()
    {
        // TODO: Play getup animation.
        agent.InternalModel.Remove(InternalState.Sitting);
        agent.InternalModel.Remove(InternalState.LyingDown);
        stopped = false;
    }

    public void Stop()
    {
        navAgent.ResetPath();
        stopped = true;
    }
}

public interface SteeringBehavior
{
    Vector3 Destination();
}

public class Pursuit : SteeringBehavior
{
    Transform target = null;
    float lead = 0f;

    public Pursuit(Transform target, float lead)
    {
        this.lead = Mathf.Abs(lead);
        this.target = target;
    }

    public Vector3 Destination() => target.position + target.forward * lead;
}

public class WanderSteer : SteeringBehavior
{
    Transform agentTransform = null;
    float radius = 3f;
    float circleOffset = .5f;
    float maxAngleChange = 180f;

    public WanderSteer(Transform agentTransform)
    {
        this.agentTransform = agentTransform;
    }

    public Vector3 Destination()
    {
        var position = agentTransform.position;
        var forward = agentTransform.forward;
        var changeVector = Quaternion.AngleAxis(maxAngleChange * RandomBinomial(), Vector3.up) * forward * radius;
        var circleCenter = forward * circleOffset;
        return position + circleCenter + changeVector;
    }

    public static float RandomBinomial()
    {
        return UnityEngine.Random.Range(0f, 1f) - UnityEngine.Random.Range(0f, 1f);
    }
}