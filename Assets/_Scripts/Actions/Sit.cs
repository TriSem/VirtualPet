using UnityEngine;

public class Sit : Reaction, IAudioReciever
{
    [SerializeField, Range(0, 100)]
    int learnedAfterRepitition = 10;

    [SerializeField] new BoxCollider collider = null;

    bool learned = false;
    bool learningEnabled = false;

    int reinforcementCount = 0;

    [SerializeField] Transform icon = null;

    public override void Cancel()
    {
        if(!learned)
        {
            icon.gameObject.SetActive(false);
            learningEnabled = false;
        }

        // TODO: Play getup animation.
        Status = ActionStatus.Inactive;
    }

    public void RecieveSignal(Command command)
    {
        if (learned && command == Command.Sit)
            Triggered = true;
        if (learningEnabled && command == Command.Sit)
        {
            reinforcementCount++;
            if (reinforcementCount >= learnedAfterRepitition)
                learned = true;
        }
    }

    public override void UseAction(PetAgent agent)
    {
        Triggered = false;
        Status = ActionStatus.Ongoing;
        if(!learned)
        {
            learningEnabled = true;
            icon.gameObject.SetActive(true);
        }

        agent.NavAgent.isStopped = true;
        // TODO: Play sitting animation
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treat"))
            Triggered = true;
    }
}
