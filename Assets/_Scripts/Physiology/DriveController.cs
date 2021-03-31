using UnityEngine;

[RequireComponent(typeof(PetAgent))]
public class DriveController : MonoBehaviour
{
    [SerializeField] float hungerBuildUp = 0.2f;
    [SerializeField] float fatigueBuildUp = 0.1f;
    [SerializeField] float minimum = -10f;
    [SerializeField] float maximum = 10f;

    float fatigue = 0f;

    [SerializeField] PetAgent agent = default;

    void Start()
    {
        agent = GetComponent<PetAgent>();
        agent.DriveVector.SetValue(Drive.Affection, 0.2f);
        agent.DriveVector.SetValue(Drive.Play, 0.1f);
    }

    void Update()
    {
        fatigue += fatigueBuildUp * Time.deltaTime;
        fatigue = Mathf.Min(maximum, fatigue);

        agent.DriveVector.SetValue(Drive.Sleep, fatigue / 10f);
    }

    public void ReduceFatigue(float amount)
    {
        fatigue -= amount;
        fatigue = Mathf.Max(minimum, fatigue);
    }
}

