using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DriveVector
{
    Dictionary<Drive, float> drives = new Dictionary<Drive, float>();

    public DriveVector()
    {
        var enumValues = (Drive[]) Enum.GetValues(typeof(Drive));
        foreach (var drive in enumValues)
        {
            drives.Add(drive, 0f);
        }
    }

    public float CalculateUtility(List<Outcome> outcomes)
    {
        float utility = 0f;
        foreach(var outcome in outcomes)
        {
            float value = drives[outcome.Drive];
            utility += value * outcome.value;
        }
        return utility;
    }

    public void SetValue(Drive drive, float value)
    {
        drives[drive] = value;
    }

    public float GetValue(Drive drive)
    {
        return drives[drive];
    }
}

[Serializable]
public struct Outcome
{
    [SerializeField] Drive drive;
    [SerializeField] public float value;

    public Drive Drive => drive;
}
