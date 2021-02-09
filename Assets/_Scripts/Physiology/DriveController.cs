using UnityEngine;

[RequireComponent(typeof(PetAgent))]
public class DriveController : MonoBehaviour
{
    [SerializeField] float hungerBuildUp = 0.2f;
    [SerializeField] float fatigueBuildUp = 0.1f;
    [SerializeField] float minimum = -10f;
    [SerializeField] float maximum = 10f;

    float fatigue = 0f;
    float hunger = 0f;


    [SerializeField] PetAgent agent = default;

    void Start()
    {
        agent = GetComponent<PetAgent>();
        agent.DriveVector.SetValue(Drive.Affection, 0.2f);
        agent.DriveVector.SetValue(Drive.Fun, 0.1f);
    }

    void Update()
    {
        if (agent.Motor.ActivityLevel != ActivityLevel.RESTING)
        {
            fatigue += fatigueBuildUp * Time.deltaTime;
            fatigue = Mathf.Min(maximum, fatigue);
        }

        hunger += hungerBuildUp * Time.deltaTime;
        hunger = Mathf.Min(maximum, hunger);

        agent.DriveVector.SetValue(Drive.Energy, fatigue);
        agent.DriveVector.SetValue(Drive.Food, hunger);
    }

    public void ReduceHunger(float amount)
    {
        hunger -= amount;
        hunger = Mathf.Max(minimum, hunger);
    }

    public void ReduceFatigue(float amount)
    {
        fatigue -= amount;
        fatigue = Mathf.Max(minimum, fatigue);
    }
}

