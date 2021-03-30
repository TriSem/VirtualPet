using UnityEngine;

public class Wander : Behavior
{
    MotorSystem motor = null;

    public override void Cancel()
    {
        Status = BehaviorState.Inactive;
        motor.Stop();
    }

    public override void Use(PetAgent agent)
    {
        Status = BehaviorState.Ongoing;
        motor = agent.Motor;
        motor.Wander();
    }

    void Update()
    {
    }
}
