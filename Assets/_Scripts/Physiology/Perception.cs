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
        TrimDeletedObjects();
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

    void TrimDeletedObjects()
    {
        HashSet<WorldObject> deletedObjects = new HashSet<WorldObject>();
        foreach(var visual in perceptions[PerceptionType.Visual])
        {
            if (visual == null)
                deletedObjects.Add(visual);
        }

        perceptions[PerceptionType.Visual].ExceptWith(deletedObjects);
    }
}

public enum PerceptionType
{
    Visual,
    Audio,
    Smell
}
