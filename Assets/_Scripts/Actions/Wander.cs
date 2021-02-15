using UnityEngine;
using UnityEngine.AI;

public class Wander : ActionObject
{
    MotorSystem motor = null;
    float waitFor = 1f;
    float waitTime = 0f;

    public override void Cancel()
    {
        Status = ActionStatus.Inactive;
    }

    public override void UseAction(PetAgent agent)
    {
        motor = agent.Motor;
        var moveTo = Random.onUnitSphere * Random.Range(1f, 10f) + transform.position;
        if(NavMesh.SamplePosition(moveTo, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            motor.MoveTo(hit.position);
        }
        waitTime = waitFor + Time.time;
        Status = ActionStatus.Ongoing;
    }

    void Update()
    {
        if (
            Status == ActionStatus.Ongoing && 
            motor.Arrived &&
            Time.time > waitTime)
            Cancel();
    }
}
