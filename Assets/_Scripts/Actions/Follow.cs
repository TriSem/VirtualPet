using UnityEngine.AI;
using UnityEngine;

public class Follow : ActionObject
{
    NavMeshAgent navAgent = null;

    public override void Cancel()
    {
        Status = ActionStatus.Inactive;
    }

    public override void UseAction(PetAgent agent)
    {
        navAgent = agent.NavAgent;
        Status = ActionStatus.Ongoing;
    }

    void Update()
    {
        if (Status == ActionStatus.Ongoing)
        {
            navAgent.SetDestination(transform.position);
            if(Vector3.Distance(
                navAgent.transform.position,
                transform.position) <= navAgent.stoppingDistance)
            {
                Cancel();
            }
        }
    }
}
