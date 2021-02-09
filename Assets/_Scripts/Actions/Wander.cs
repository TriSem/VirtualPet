using UnityEngine;
using UnityEngine.AI;

public class Wander : ActionObject
{
    NavMeshAgent navAgent = null;
    float waitFor = 5f;
    float waitTime = 0f;

    public override void Cancel()
    {
        Status = ActionStatus.Inactive;
    }

    public override void UseAction(PetAgent agent)
    {
        navAgent = agent.NavAgent;
        var moveTo = Random.onUnitSphere * Random.Range(1f, 10f) + transform.position;
        if(NavMesh.SamplePosition(moveTo, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            navAgent.destination = hit.position;
        }
        waitTime = waitFor + Time.time;
        Status = ActionStatus.Ongoing;
    }

    void Update()
    {
        if (
            Status == ActionStatus.Ongoing && 
            Vector3.Distance(navAgent.transform.position, navAgent.destination) < navAgent.stoppingDistance &&
            Time.time > waitTime)
            Cancel();
    }
}
