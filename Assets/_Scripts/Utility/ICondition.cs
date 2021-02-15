using UnityEngine.AI;

interface ICondition
{
    bool Met { get; }
}

class DestinationCondition : ICondition
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
