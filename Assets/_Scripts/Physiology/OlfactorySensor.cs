using System.Collections.Generic;
using UnityEngine;

// Sensor for smelling objects.
public class OlfactorySensor : Sensor
{
    public override HashSet<WorldObject> GetPercievedObjects()
    {
        var objects = WorldBlackboard.Instance.GetObjects();
        var percievedObjects = new HashSet<WorldObject>();
        foreach(var worldObject in objects)
        {
            float distance = Vector3.Distance(worldObject.transform.position, this.transform.position);
            if (PercievableWhenAttenuated(worldObject.Smelliness, distance))
                percievedObjects.Add(worldObject);
        }
        return percievedObjects;
    }
}
