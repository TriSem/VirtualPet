using System.Collections.Generic;
using UnityEngine;

public class Perception : MonoBehaviour
{
    [SerializeField] List<Sensor> sensors;

    public HashSet<WorldObject> Poll()
    {
        var result = new HashSet<WorldObject>(GetMemorableObjects());
        foreach(var sensor in sensors)
        {
            result.UnionWith(sensor.GetPercievedObjects());
        }
        return result;
    }

    // Returns all objects that don't change position and
    // count as being remembered by the pet.
    HashSet<WorldObject> GetMemorableObjects()
    {
        var memorables = new HashSet<WorldObject>();
        foreach (var worldObject in WorldBlackboard.Instance.GetObjects())
        {
            if (worldObject.Memorable)
                memorables.Add(worldObject);
        }
        return memorables;
    }    
}