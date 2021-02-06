using UnityEngine;

[RequireComponent(typeof(WorldObject))]
public abstract class Trait : MonoBehaviour
{
}

public class Food : Trait
{
    [SerializeField] float maximumAmount = 100f;
    public float RemainingAmount { get; private set; }


    public void Fill(float amount)
    {
        RemainingAmount = Mathf.Min(maximumAmount, RemainingAmount + amount);
    }

    /// <summary>
    /// Reduce amount of food left by up to the specified amount.
    /// </summary>
    /// <returns>Actual amount of consumed food.</returns>
    public float Consume(float amount)
    {
        if (RemainingAmount >= amount)
        {
            RemainingAmount -= amount;
            return amount;
        }
        else
        {
            float consumed = RemainingAmount;
            RemainingAmount = 0;
            return consumed;
        }
    }
}