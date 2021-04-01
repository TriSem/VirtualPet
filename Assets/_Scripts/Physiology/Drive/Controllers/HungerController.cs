using UnityEngine;

public class HungerController : MonoBehaviour
{
    [SerializeField] float satedRatio = 1f;
    [SerializeField] float peckishRatio = 0.5f;
    [SerializeField] float starvingRatio = 0f;
    [SerializeField, Range(1f, 1000f)]
    float maxFill = 100f;

    [SerializeField, Range(0f, 1000f)]
    float decayPerSecond = 0.5f;

    [SerializeField] PetAgent agent = null;


    float currentFill = 0f;

    public float FillRatio => currentFill / maxFill;

    void Update()
    {
        ChangeFill(-decayPerSecond * Time.deltaTime);
        var drives = agent.DriveVector;
        if (FillRatio <= starvingRatio)
            drives.SetValue(Drive.Food, 2f);
        else if (FillRatio <= peckishRatio)
            drives.SetValue(Drive.Food, 1f);
        else if (FillRatio >= satedRatio)
            drives.SetValue(Drive.Food, -1f);
        else
            drives.SetValue(Drive.Food, 1f);
    }

    public void ChangeFill(float value)
    {
        currentFill += value;
        Mathf.Clamp(currentFill, 0, maxFill);
    }
}
