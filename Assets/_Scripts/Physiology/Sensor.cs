using System.Collections.Generic;
using UnityEngine;

public abstract class Sensor : MonoBehaviour
{
    [SerializeField] protected float minimumSignalStrength = 1f;
    [SerializeField] protected float maximumRange = 20f;
    [SerializeField, Range(0f, 1f)] protected float attenuation = 1f;

    public abstract HashSet<WorldObject> GetPercievedObjects();

    protected bool PercievableWhenAttenuated(float signalStrength, float distance) =>
        signalStrength * Mathf.Pow(attenuation, distance) >= minimumSignalStrength;
}
