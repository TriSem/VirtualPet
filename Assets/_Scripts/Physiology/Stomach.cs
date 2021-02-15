using UnityEngine;

public class Stomach : MonoBehaviour
{
    [SerializeField, Range(1f, 1000f)] 
    float maxFill = 100f;
    
    [SerializeField, Range(1f, 1000f)]
    float avoidFoodAtFill = 90f;

    [SerializeField, Range(0f, 1000f)]
    float decayPerSecond = 0.5f;

    [SerializeField] 
    PetAgent agent = null;

    
    float currentFill = 0f;

    void Update()
    {
        ChangeFill(-decayPerSecond * Time.deltaTime);

        if (currentFill >= avoidFoodAtFill)
            agent.DriveVector.SetValue(Drive.Food, -1f);
        else
            agent.DriveVector.SetValue(Drive.Food, 1f);
    }

    public void ChangeFill(float value)
    {
        currentFill += value;
        Mathf.Clamp(currentFill, 0, maxFill);
    }
}
