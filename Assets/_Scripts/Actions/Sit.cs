using UnityEngine;

public class Sit : Reaction, IAudioReciever
{
    [SerializeField, Range(0, 100)]
    int learnedAfterRepitition = 10;

    [SerializeField, Range(1f, 100f)]
    float minimumSitTime = 1f;

    [SerializeField, Range(1f, 100f)]
    float maximumSitTime = 5f;

    [SerializeField]
    new BoxCollider collider = null;

    bool learned = false;
    bool learningEnabled = false;
    float stopSittingTime = 0f;

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

        stopSittingTime = Time.time + Random.Range(minimumSitTime, maximumSitTime);

        agent.NavAgent.isStopped = true;
        // TODO: Play sitting animation
    }

    void Update()
    {
        if(Status == ActionStatus.Ongoing)
        {
            if (Time.time > stopSittingTime)
                Cancel();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treat"))
            Triggered = true;
    }
}
