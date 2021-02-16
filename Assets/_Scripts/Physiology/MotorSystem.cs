using UnityEngine;
using UnityEngine.AI;

public class MotorSystem : MonoBehaviour
{
    [SerializeField] NavMeshAgent navAgent = null;
    [SerializeField] float baseSpeed = 3f;
    [SerializeField] float maximumSpeed = 6f;
    [SerializeField] float baseAngularSpeed = 135f;
    [SerializeField] float maximumAngularSpeed = 270f;
    [SerializeField] float baseStoppingDistance = 2f;

    InteractionCondition interactionCondition;
    DestinationCondition destinationCondition;
    ICondition currentCondition;

    public bool Arrived => currentCondition.Met;


    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = baseSpeed;
        navAgent.angularSpeed = baseAngularSpeed;
        navAgent.stoppingDistance = baseStoppingDistance;
        destinationCondition = new DestinationCondition(navAgent);
        currentCondition = destinationCondition;
    }

    void Update()
    {
        LimitCurrentSpeed();

        if(Arrived)
        {
            navAgent.ResetPath();
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

    public void GoInteract(Interaction interaction)
    {
        interactionCondition = new InteractionCondition(interaction);
        navAgent.stoppingDistance = 0.5f;
        navAgent.destination = interaction.transform.position;
        currentCondition = interactionCondition;
    }

    public void MoveTo(Vector3 position)
    {
        navAgent.destination = position;
        navAgent.stoppingDistance = baseStoppingDistance;
        currentCondition = destinationCondition;
    }

    public void Stop()
    {
        navAgent.ResetPath();
        currentCondition = destinationCondition;
    }
}