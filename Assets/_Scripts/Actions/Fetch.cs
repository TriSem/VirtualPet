using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fetch : ActionObject
{
    PetAgent agent;

    public override void Cancel()
    {
        throw new System.NotImplementedException();
    }

    public override void Use(PetAgent agent)
    {
        throw new System.NotImplementedException();
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override bool PreconditionsMet(InternalModel internalModel)
    {
        return internalModel.Contains(InternalState.Carrying);
    }
}
