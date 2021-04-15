using System.Collections.Generic;
using UnityEngine;

public class AudioSensor : Sensor
{
    public override HashSet<WorldObject> GetPercievedObjects()
    {
        var objects = WorldBlackboard.Instance.GetObjects();
        var percievedObjects = new HashSet<WorldObject>();
        foreach(var worldObject in objects)
        {
            float distance = Vector3.Distance(worldObject.transform.position, this.transform.position);
            if (PercievableWhenAttenuated(worldObject.Audibility, distance))
                percievedObjects.Add(worldObject);
        }
        return percievedObjects;
    }
}
