using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : ActionObject
{
    PetAgent agent = null;

    [SerializeField] bool refillable = false;
    [SerializeField] float edibleDistance = 1f;

    public override void Cancel()
    {
        Status = ActionStatus.Cancelled;
    }

    public override void UseAction(PetAgent agent)
    {
        this.agent = agent;  
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
        if(Status == ActionStatus.Ongoing)
        {
            float zDistanceToPivot = agent.BoundingBox.bounds.extents.z;
            var pointOnBounds = agent.transform.position + agent.transform.forward * zDistanceToPivot;
            if(Vector3.Distance(pointOnBounds, transform.position) < 1f)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
