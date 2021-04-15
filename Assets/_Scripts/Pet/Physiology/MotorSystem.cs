using UnityEngine;
using UnityEngine.AI;

public class MotorSystem : MonoBehaviour
{
    [SerializeField] 
    NavMeshAgent navAgent = null;

    [SerializeField] 
    Animator animator = null;

    [SerializeField] 
    PetAgent agent = null;

    [SerializeField] 
    float baseSpeed = 3f;

    [SerializeField] 
    float maximumSpeed = 6f;

    [SerializeField] 
    float baseAngularSpeed = 135f;

    [SerializeField] 
    float maximumAngularSpeed = 270f;

    [SerializeField] 
    float baseStoppingDistance = 2f;

    bool stopped = true;

    SteeringBehavior currentBehavior = null;
    SteeringBehavior nullSteer;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        ResetNavAgent();
        nullSteer = new NullSteer(navAgent.transform);
    }

    void Update()
    {
        if(!stopped)
        {
            navAgent.destination = currentBehavior.GetDestination();
        }
    }

    void ResetNavAgent()
    {
        navAgent.speed = baseSpeed;
        navAgent.angularSpeed = baseAngularSpeed;
        navAgent.updateRotation = true;
        navAgent.updatePosition = true;
        navAgent.stoppingDistance = baseStoppingDistance;
    }


    public void Pursue(Transform target, float pursueLead)
    {
        stopped = false;
        navAgent.speed = maximumSpeed;
        navAgent.angularSpeed = maximumAngularSpeed;
        navAgent.updateRotation = true;
        navAgent.updatePosition = true;
        currentBehavior = new Pursuit(target, pursueLead);
    }

    public void Wander()
    {
        ResetNavAgent();
        animator.SetBool("Walking", true);
        stopped = false;
        currentBehavior = new WanderSteer(navAgent.transform);
    }
    
    public void Follow(Transform target)
    {
        ResetNavAgent();
        animator.SetBool("Walking", true);
        stopped = false;
        currentBehavior = new Pursuit(target, 0f);
    }

    public void MoveTo(Vector3 destination)
    {
        ResetNavAgent();
        animator.SetBool("Walking", true);
        stopped = false;
        currentBehavior = new MoveTo(destination);
        navAgent.stoppingDistance = 0.1f;
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
        animator.SetBool("Sitting", true);
    }

    public void LieDown()
    {
        stopped = true;
        agent.InternalModel.Add(InternalState.LyingDown);
        animator.SetBool("LyingDown", true);
    }

    public void GetUp()
    {
        agent.InternalModel.Remove(InternalState.Sitting);
        agent.InternalModel.Remove(InternalState.LyingDown);
        animator.SetBool("Sitting", false);
        animator.SetBool("LyingDown", false);
        stopped = false;
    }

    public void ShakeHead()
    {
        animator.SetTrigger("ShakeHead");
    }

    public void StartWaggingTail()
    {
        animator.SetBool("WaggingTail", true);
    }

    public void StopWaggingTail()
    {
        animator.SetBool("WaggingTail", false);
    }

    public void Stop()
    {
        navAgent.ResetPath();
        animator.SetBool("Walking", false);
        currentBehavior = nullSteer;
        stopped = true;
    }

    public bool AtDestination() => navAgent.stoppingDistance >= navAgent.remainingDistance;
}

public interface SteeringBehavior
{
    Vector3 GetDestination();
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

    public Vector3 GetDestination() => target.position + target.forward * lead;
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

    public Vector3 GetDestination()
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

public class MoveTo : SteeringBehavior
{
    Vector3 destination;

    public MoveTo(Vector3 destination)
    {
        this.destination = destination;
    }

    public Vector3 GetDestination() => destination;
}

public class NullSteer : SteeringBehavior
{
    Transform transform;

    public NullSteer(Transform transform) 
    {
        this.transform = transform;
    }

    public Vector3 GetDestination() => transform.position;
}

