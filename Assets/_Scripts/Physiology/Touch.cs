using System.Collections.Generic;
using UnityEngine;

// Component that tracks wether the pet is being touched or not.
public class Touch : MonoBehaviour
{
    [SerializeField] 
    float tickRate = 0.2f;

    [SerializeField] 
    List<TouchReceptor> touchReceptors = null;

    [SerializeField] 
    PetAgent agent = null;

    float nextTick = 0f;

    void Update()
    {
        float time = Time.time;
        if(time >= nextTick)
        {
            nextTick = time + tickRate;
            AdjustInternalState();
        }
    }

    void AdjustInternalState()
    {
        bool touched = PetIsTouched();
        if (touched && !agent.InternalModel.Contains(InternalState.BeingPet))
            agent.InternalModel.Add(InternalState.BeingPet);
        else if (!touched && agent.InternalModel.Contains(InternalState.BeingPet))
            agent.InternalModel.Remove(InternalState.BeingPet);
    }

    public bool PetIsTouched()
    {
        foreach (var receptor in touchReceptors)
        {
            if (receptor.Fired)
                return true;
        }
        return false;
    }
}
