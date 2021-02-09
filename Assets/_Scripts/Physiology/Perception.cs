using System;
using System.Collections.Generic;
using UnityEngine;

public class Perception : MonoBehaviour
{
    [SerializeField] float visiblityThreshold = 1f;
    [SerializeField] float audibilityThreshold = 1f;
    [SerializeField] float smellThreshold = 1f;

    public float VisibilityThreshold => visiblityThreshold;
    public float AudibilityThreshold => audibilityThreshold;
    public float SmellThreshold => smellThreshold;

    readonly Dictionary<PerceptionType, HashSet<WorldObject>> perceptions = 
        new Dictionary<PerceptionType, HashSet<WorldObject>>();

    void Start()
    {
        var perceptionTypes = (PerceptionType[])Enum.GetValues(typeof(PerceptionType));
        foreach(var type in perceptionTypes)
        {
            perceptions.Add(type, new HashSet<WorldObject>());
        }
    }

    void Update()
    {
        var visuals = perceptions[PerceptionType.Visual];
        foreach (var worldObject in visuals)
        {
            if(worldObject.Visibility < visiblityThreshold)
                visuals.Remove(worldObject);
        }

        var audibles = perceptions[PerceptionType.Audio];
        foreach (var worldObject in audibles)
        {
            if (worldObject.Audibility < audibilityThreshold)
                audibles.Remove(worldObject);
        }

        var smells = perceptions[PerceptionType.Smell];
        foreach (var worldObject in smells)
        {
            if (worldObject.Smelliness < smellThreshold)
                smells.Remove(worldObject);
        }
    }

    public void Add(PerceptionType type, WorldObject worldObject)
    {
        try
        {
            perceptions[type].Add(worldObject);
        }
        catch
        {
            return;
        }
    }

    public void Remove(PerceptionType type, WorldObject worldObject)
    {
        try
        {
            perceptions[type].Remove(worldObject);
        }
        catch
        {
            return;
        }
    }

    public void RemoveFromPerception(WorldObject worldObject)
    {
        foreach(var perception in perceptions)
        {
            perception.Value.Remove(worldObject);
        }
    }

    public HashSet<WorldObject> GetWorldObjects()
    {
        HashSet<WorldObject> worldObjects = new HashSet<WorldObject>();
        foreach(var element in perceptions)
        {
            worldObjects.UnionWith(element.Value);
        }
        return worldObjects;
    }
}

public enum PerceptionType
{
    Visual,
    Audio,
    Smell
}
