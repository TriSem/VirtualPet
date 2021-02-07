﻿using UnityEngine;

public class GastricTract : MonoBehaviour
{
    [SerializeField] float stomachVolume = 100f;
    [SerializeField] float digestionRate = 2f;
    [SerializeField] float throwUpRate = 30f;
    [SerializeField] float foodIntakeRate = 10f;
    [SerializeField] float desiredFillRatio = 0.9f;

    float stomachContent = 0;
    bool throwingUp = false;
    bool eating = false;
    Food food = null;

    public float FillRatio => stomachContent / stomachVolume;
    public float DesiredFillRatio => desiredFillRatio;

    public void Update()
    {
        if (stomachContent == stomachVolume)
            throwingUp = true;

        if(throwingUp)
        {
            ReduceContent(throwUpRate * Time.deltaTime);
        }
        else if(eating)
        {
            float bite = foodIntakeRate * Time.deltaTime;
            AddContent(food.Consume(bite));
        }
    }

    public void Eat(Food food)
    {
        this.food = food;
        eating = true;
    }

    public void StopEating() => eating = false;

    public void ThrowUp() => throwingUp = true;

    void ReduceContent(float amount)
    {
        stomachContent = Mathf.Max(0, stomachContent - amount);
    }

    void AddContent(float amount)
    {
        stomachContent = Mathf.Min(stomachVolume, stomachContent + amount);
    }
}