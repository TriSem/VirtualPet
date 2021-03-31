using UnityEngine;
using UnityEngine.AI;

public interface ICondition
{
    bool Met { get; }
}

public class AndCondition : ICondition
{
    ICondition condition1, condition2;

    public AndCondition(ICondition condition1, ICondition condition2)
    {
        this.condition1 = condition1;
        this.condition2 = condition2;
    }

    public bool Met => condition1.Met && condition2.Met;
}

public class OrCondition : ICondition
{
    ICondition condition1, condition2;

    public OrCondition(ICondition condition1, ICondition condition2)
    {
        this.condition1 = condition1;
        this.condition2 = condition2;
    }


    public bool Met => condition1.Met || condition2.Met;
}

public class NotCondition : ICondition
{
    ICondition condition;

    public NotCondition(ICondition condition) => this.condition = condition;

    public bool Met => !condition.Met;
}


public class DestinationCondition : ICondition
{
    MotorSystem motor;

    public DestinationCondition(MotorSystem motor) => this.motor = motor;

    public bool Met => motor.AtDestination();
}

public class InteractionCondition : ICondition
{
    Interaction interaction;

    public InteractionCondition(Interaction interaction) => this.interaction = interaction;

    public bool Met => interaction.PetInRange;
}

public class WithinDistanceCondition : ICondition
{
    float squareDistance;
    Transform transform, other;


    public WithinDistanceCondition(Transform transform, Transform other, float distance)
    {
        squareDistance = distance * distance;
        this.transform = transform;
        this.other = other;
    }

    public bool Met => (transform.position - other.position).sqrMagnitude <= squareDistance;
}

public class TimerCondition : ICondition
{
    public float StopTime { get; set; } = 0f;

    public TimerCondition() {}

    public TimerCondition(float stopTime) => StopTime = stopTime;

    public bool Met => Time.time >= StopTime;
}

public class MetCondition : ICondition
{
    public bool Met => true;
}
