using UnityEngine;

public class SleepController : MonoBehaviour
{
    [SerializeField] 
    float maxTiredness = 100f;

    [SerializeField] 
    float buildUpRate = 1f;

    [SerializeField] 
    float recoveryRate = 1f;

    [SerializeField] 
    float avoidSleepBelow = 20f;

    [SerializeField] 
    PetAgent agent = null;

    float tiredness = 0f;

    void Update()
    {
        if (agent.InternalModel.Contains(InternalState.Sleeping))
        {
            tiredness -= recoveryRate * Time.deltaTime;
        }
        else
        {
            tiredness += buildUpRate * Time.deltaTime;
        }
        tiredness = Mathf.Clamp(tiredness, 0f, maxTiredness);

        // The dog will avoid sleep below a certain threshold.
        // After that the need to sleep will ramp up linearly.
        if (tiredness < avoidSleepBelow)
            agent.DriveVector.SetValue(Drive.Sleep, -2);
        else
            agent.DriveVector.SetValue(Drive.Sleep, 
                Mathf.Lerp(2f, 0f, (maxTiredness - tiredness) / (maxTiredness - avoidSleepBelow)));
    }
}
