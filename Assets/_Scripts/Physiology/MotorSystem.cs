using UnityEngine;
using UnityEngine.AI;

public class MotorSystem : MonoBehaviour
{
    [SerializeField] NavMeshAgent navAgent = null;
    [SerializeField] float baseSpeed = 3f;
    [SerializeField] float maximumSpeed = 6f;
    [SerializeField] float baseAngularSpeed = 45f;
    [SerializeField] float maximumAngularSpeed = 90;

    public MotorState State { get; private set; } = MotorState.STANDING;
    public ActivityLevel ActivityLevel { get; private set; } = ActivityLevel.RESTING;

    void Start()
    {
        navAgent.speed = baseSpeed;
        navAgent.angularSpeed = baseAngularSpeed;
    }

    void Update()
    {
        DetermineActivityLevel();
        LimitCurrentSpeed();
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

    void DetermineActivityLevel()
    {
        if (State == MotorState.LYING_DOWN || State == MotorState.SITTING)
            ActivityLevel = ActivityLevel.RESTING;
        else if (State == MotorState.STANDING)
            ActivityLevel = ActivityLevel.LIGHT;
        else
        {
            float third = (maximumSpeed - baseSpeed) / 3;
            if (navAgent.speed < baseSpeed + third)
                ActivityLevel = ActivityLevel.LIGHT;
            else if (navAgent.speed < baseSpeed + third * 2)
                ActivityLevel = ActivityLevel.MODERATE;
            else
                ActivityLevel = ActivityLevel.INTENSE;
        }
    }

    void LimitCurrentSpeed()
    {
        float limit = DetermineSpeedLimit();
        navAgent.speed = Mathf.Min(navAgent.speed, limit);
        AdjustAngularSpeed();   
    }
}

public enum MotorState
{
    LYING_DOWN,
    SITTING,
    STANDING,
    MOVING
}

public enum ActivityLevel
{
    RESTING,
    LIGHT,
    MODERATE,
    INTENSE
}