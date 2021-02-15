using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] float maximum = 100f;
    [SerializeField] float recoveryRate = 1f;
    float current = 0;

    public float Maximum => maximum;
    public float Current => current;

    void Update()
    {
    }
}
