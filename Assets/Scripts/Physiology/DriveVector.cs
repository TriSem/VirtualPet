using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DriveVector
{
    Dictionary<Drive, float> drives;

    public DriveVector()
    {
        var enumValues = (Drive[]) Enum.GetValues(typeof(Drive));
        foreach (var drive in enumValues)
        {
            drives.Add(drive, 0f);
        }
    }

    float CalculateUtility(List<DriveValue> driveValues)
    {
        float utility = 0f;
        foreach(var driveValue in driveValues)
        {
            float value = drives[driveValue.Drive];
            utility += value * driveValue.value;
        }
        return utility;
    }

    void SetValue(Drive drive, float value)
    {
        drives[drive] = value;
    }
}

[Serializable]
public struct DriveValue
{
    [SerializeField] Drive drive;
    [SerializeField] public float value;

    public Drive Drive => drive;
}
