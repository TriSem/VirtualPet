using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : ActionObject
{
    [SerializeField] bool refillable = false;

    public override void Cancel()
    {
        Status = ActionStatus.Cancelled;
    }

    public override void UseAction(PetAgent agent)
    {
        if (!IsUsable())
            return;

        else
            Status = ActionStatus.Ongoing;
    }

    void GetEaten()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
