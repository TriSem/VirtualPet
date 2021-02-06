using System;
using System.Collections.Generic;
using UnityEngine;

public class DriveSet : MonoBehaviour
{
    [SerializeField] List<IDriveController> driveControllers = null;
    [SerializeField] float recordInterval = 1f;
    [SerializeField] float historyLength = 30f;
    Dictionary<Drive, float> drives = new Dictionary<Drive, float>();
    Dictionary<Drive, List<HistoryRecord>> history = new Dictionary<Drive, List<HistoryRecord>>();
    float lastRecordTime = 0f;

    public float DriveValue(Drive drive) => drives[drive];

    void Awake()
    {
        var enumValues = (Drive[])Enum.GetValues(typeof(Drive));
        foreach (Drive drive in enumValues)
        {
            drives.Add(drive, 0f);
            history.Add(drive, new List<HistoryRecord>());
        }
    }

    void Update()
    {
        UpdateDrives();
        UpdateHistory();
    }

    void UpdateDrives()
    {
        foreach (var controller in driveControllers)
        {
            drives[controller.ControlledDrive] = controller.ErrorValue * controller.Modifier;
        }
    }

    void UpdateHistory()
    {
        float time = Time.time;
        if (lastRecordTime + recordInterval < time)
        {
            foreach (var entry in history)
            {
                float value = drives[entry.Key];
                var queue = entry.Value;
                HistoryRecord record = new HistoryRecord(time, value);
                queue.Add(record);

                HistoryRecord oldestRecord = queue[0];
                if (oldestRecord.Time + historyLength < time)
                    queue.RemoveAt(0);
            }
            lastRecordTime = time;
        }
    }

    public float CalculateDriveChange(Drive drive, float from, float to)
    {
        float change = 0f;
        var records = history[drive];
        int index1 = GetNearestIndex(records, from);
        int index2 = GetNearestIndex(records, to);

        if (index1 < 0 || index2 < 0)
            return change;

        int count = index2 - index1;
        records = records.GetRange(index1, count);
        float total = 0f;
        foreach(var record in records)
        {
            total += record.Value;
        }
        total /= records.Count;
        change = total - records[0].Value;
        return change;
    }

    int GetNearestIndex(List<HistoryRecord> records, float time)
    {
        int index = -1;
        for (int i = 0; i < records.Count; i++)
        {
            if (records[i].Time >= time)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public float GetValue(Drive drive) => drives[key: drive];

    public Drive StrongestDrive()
    {
        var strongest = new KeyValuePair<Drive, float>();
        foreach (var entry in drives)
        {
            float currentStrongest = Mathf.Abs(strongest.Value);
            float magnitude = Mathf.Abs(entry.Value);
            if (magnitude > currentStrongest)
                strongest = entry;
        }

        return strongest.Key;
    }

    public struct HistoryRecord
    {
        public float Time { get; private set; }
        public float Value { get; private set; }

        public HistoryRecord(float time, float value)
        {
            this.Time = time;
            this.Value = value;
        }
    }
}
