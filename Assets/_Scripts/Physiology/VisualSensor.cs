using System.Collections.Generic;
using UnityEngine;

public class VisualSensor : Sensor
{
    [SerializeField] float visionAngle = 45f;
    [SerializeField] Light visualization;

    public override HashSet<WorldObject> GetPercievedObjects()
    {
        var objects = WorldBlackboard.Instance.GetObjects();
        var percievedObjects = new HashSet<WorldObject>();
        foreach(var worldObject in objects)
        {
            if (CanPercieve(worldObject))
                percievedObjects.Add(worldObject);
        }
        return percievedObjects;
    }

    bool CanPercieve(WorldObject worldObject)
    {
        var delta = worldObject.transform.position - transform.position;
        var distance = delta.magnitude;
        if (delta.magnitude > maximumRange || 
            Vector3.Angle(transform.forward, delta) > visionAngle)
            return false;

        return PercievableWhenAttenuated(worldObject.Visibility, distance);
    }

    void Start()
    {
        visualization.range = maximumRange;
        visualization.innerSpotAngle = visionAngle;
        visualization.spotAngle = visionAngle;
    }
}