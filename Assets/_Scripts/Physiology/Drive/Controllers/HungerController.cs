using UnityEngine;

public class HungerController : MonoBehaviour
{
    [SerializeField, Range(0.01f, 1f)] 
    float satedRatio = 1f;

    [SerializeField, Range(0.01f, 1f)] 
    float peckishRatio = 0.5f;

    [SerializeField, Range(0.01f, 1f)] 
    float starvingRatio = 0f;

    [SerializeField, Range(1f, 1000f)]
    float maxFill = 100f;

    [SerializeField, Range(0f, 1000f)]
    float decayPerSecond = 0.5f;

    [SerializeField] 
    PetAgent agent = null;


    float currentFill = 0f;

    public float FillRatio => currentFill / maxFill;

    void Update()
    {
        ChangeFill(-decayPerSecond * Time.deltaTime);
        var drives = agent.DriveVector;
        float driveValue;
        if (FillRatio <= starvingRatio)
            driveValue = 2f;
        else if (FillRatio <= peckishRatio)
            driveValue = 1f;
        else if (FillRatio >= satedRatio)
            driveValue = -1f;
        else
            driveValue = 1f;
        drives.SetValue(Drive.Food, driveValue);
    }

    public void ChangeFill(float value)
    {
        currentFill += value;
        Mathf.Clamp(currentFill, 0, maxFill);
    }
}
