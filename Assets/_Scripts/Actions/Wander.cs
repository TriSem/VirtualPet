using UnityEngine;

public class Wander : ActionObject
{
    MotorSystem motor = null;
    [SerializeField] float speed = 2f;
    [SerializeField] float maxRotation = 90f;

    public override void Cancel()
    {
        Status = ActionStatus.Inactive;
    }

    public override void Use(PetAgent agent)
    {
    }

    void Update()
    {
        if(Status == ActionStatus.Ongoing)
        {
        }
    }

    float RandomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }
}
