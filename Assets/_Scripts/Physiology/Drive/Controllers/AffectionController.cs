using UnityEngine;

public class AffectionController : MonoBehaviour
{
    [SerializeField] 
    PetAgent agent = null;

    [SerializeField] 
    float buildUprate = 1f;

    [SerializeField] 
    float reductionRate = 1f;

    [SerializeField] 
    float maximum = 100f;

    [SerializeField]
    float startValue = 100f;

    [SerializeField] 
    float minimumNeed = -0.5f;

    [SerializeField] 
    float maximumNeed = 0.5f;

    float currentValue;

    void Start()
    {
        currentValue = startValue;    
    }

    void Update()
    {
        if (agent.InternalModel.Contains(InternalState.BeingPet))
            currentValue -= reductionRate * Time.deltaTime;
        else
            currentValue += buildUprate * Time.deltaTime;
        currentValue = Mathf.Clamp(currentValue, 0, maximum);

        var driveVector = agent.DriveVector;
        driveVector.SetValue(Drive.Affection, Mathf.Lerp(minimumNeed, maximumNeed, currentValue / maximum));
    }
}
