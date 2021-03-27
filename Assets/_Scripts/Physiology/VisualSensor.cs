using System.Collections.Generic;
using UnityEngine;

public class VisualSensor : MonoBehaviour, ISensor
{
    [SerializeField] Perception perception = null;
    [SerializeField] float minimumSignalStrength = 1f;
    [SerializeField] float maximumViewDistance = 20f;
    [SerializeField] float visionAngle = 45f;
    [SerializeField, Range(0f, 1f)] float attenuation = 1f;
    [SerializeField] Light visualization;

    public Modalities AssociatedModalities => Modalities.Visual;

    public List<WorldObject> PercievedObjects()
    {
        throw new System.NotImplementedException();
    }

    bool CanPercieve(WorldObject worldObject)
    {
        var delta = worldObject.transform.position - transform.position;
        var distance = delta.magnitude;
        if (delta.magnitude > maximumViewDistance || 
            Vector3.Angle(transform.forward, delta) > visionAngle)
            return false;

        return worldObject.Visibility * Mathf.Pow(attenuation, distance) >= 1f;
    }

    void Start()
    {
        visualization.range = maximumViewDistance;
        visualization.innerSpotAngle = visionAngle;
        visualization.spotAngle = visionAngle;
    }
}

public interface ISensor
{
    List<WorldObject> PercievedObjects();
    Modalities AssociatedModalities { get; }
}
