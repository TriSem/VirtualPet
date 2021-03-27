using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Perception : MonoBehaviour
{
    readonly StimulusMap stimuli = new StimulusMap();
    [SerializeField] List<Sensor> sensors;

    public List<Stimulus> Poll()
    {
        stimuli.Clear();
        foreach(var sensor in sensors)
        {
            var worldObjects = sensor.GetPercievedObjects();
            foreach (var worldObject in worldObjects)
            {
                if (stimuli.Contains(worldObject))
                    stimuli[worldObject].Modalities |= sensor.Modalities;
                else
                {
                    var stimulus = new Stimulus(worldObject);
                    stimulus.Modalities = sensor.Modalities;
                }
            }
        }
        return new List<Stimulus>(stimuli);
    }
}

public class Stimulus
{
    public WorldObject WorldObject { get; private set; }
    public Modalities Modalities { get; set; } = Modalities.None;

    public Stimulus(WorldObject worldObject)
    {
        WorldObject = worldObject;
    }

    public float Salience() => 1f;

}

public class StimulusMap : KeyedCollection<WorldObject, Stimulus>
{
    protected override WorldObject GetKeyForItem(Stimulus stimulus)
    {
        return stimulus.WorldObject;
    }
}


[Flags]
public enum Modalities
{
    None = 0,
    Visual = 1,
    Audio = 2,
    Smell = 4
}
