using UnityEngine;

public class Wander : ActionObject
{
    MotorSystem motor = null;

    public override void Cancel()
    {
        Status = ActionStatus.Inactive;
        motor.Stop();
    }

    public override void Use(PetAgent agent)
    {
        Status = ActionStatus.Ongoing;
        motor = agent.Motor;
        motor.Wander();
    }

    void Update()
    {
    }
}
