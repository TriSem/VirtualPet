using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] float maximum = 100f;
    [SerializeField] float recoveryRate = 1f;
    [SerializeField] float moderateActivityCost = 1f;
    [SerializeField] float intenseActivityCost = 2f;
    float current = 0;

    ActivityLevel activityLevel = ActivityLevel.LIGHT;

    public float Maximum => maximum;
    public float Current => current;

    void Update()
    {
        float change;
        switch (activityLevel)
        {
            case ActivityLevel.MODERATE:
                change = -moderateActivityCost; break;
            case ActivityLevel.INTENSE:
                change = -intenseActivityCost; break;
            default:
                change = recoveryRate; break;
        }
        current += change * Time.deltaTime;
        current = Mathf.Clamp(current, 0, maximum);
    }
}
