using System.Collections.Generic;
using UnityEngine;

public class VisualSensor : Sensor
{
    [SerializeField] float minimumSignalStrength = 1f;
    [SerializeField] float maximumViewDistance = 20f;
    [SerializeField] float visionAngle = 45f;
    [SerializeField, Range(0f, 1f)] float attenuation = 1f;
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
        if (delta.magnitude > maximumViewDistance || 
            Vector3.Angle(transform.forward, delta) > visionAngle)
            return false;

        return worldObject.Visibility * Mathf.Pow(attenuation, distance) >= minimumSignalStrength;
    }

    void Start()
    {
        visualization.range = maximumViewDistance;
        visualization.innerSpotAngle = visionAngle;
        visualization.spotAngle = visionAngle;
    }
}