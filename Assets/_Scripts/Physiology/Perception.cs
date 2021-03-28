using System.Collections.Generic;
using UnityEngine;

public class Perception : MonoBehaviour
{
    [SerializeField] List<Sensor> sensors;

    public HashSet<WorldObject> Poll()
    {
        var result = new HashSet<WorldObject>();
        foreach(var sensor in sensors)
        {
            result.UnionWith(sensor.GetPercievedObjects());
        }
        return result;
    }
}