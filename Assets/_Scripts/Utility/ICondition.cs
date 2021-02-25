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

    public NotCondition(ICondition condition)
    {
        this.condition = condition;
    }

    public bool Met => !condition.Met;
}


public class DestinationCondition : ICondition
{
    NavMeshAgent agent;

    public DestinationCondition(NavMeshAgent agent)
    {
        this.agent = agent;
    }

    public bool Met => agent.remainingDistance <= agent.stoppingDistance;
}

public class InteractionCondition : ICondition
{
    Interaction interaction;

    public InteractionCondition(Interaction interaction)
    {
        this.interaction = interaction;
    }

    public bool Met => interaction.PetInRange;
}
