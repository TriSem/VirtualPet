using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class EventTree : KeyedCollection<EventType, PetEvent>
{
    public EventTree()
    {
        var eventTypes = (EventType[])Enum.GetValues(typeof(EventType));
        foreach(var eventType in eventTypes)
        {
            Add(new PetEvent(eventType));
        }
    }

    protected override EventType GetKeyForItem(PetEvent item)
    {
        return item.EventType;
    }

    public bool EventActive(EventQuery query)
    {
        return this[query.EventType].EventActive(query.TraitName);
    }
}

[Serializable]
public class EventQuery
{
    [SerializeField] string traitName = "None";
    [SerializeField] EventType eventType = EventType.None;
    public string TraitName => traitName;
    public EventType EventType => eventType;

    public EventQuery(EventType type, string traitName)
    {
        eventType = type;
        this.traitName = traitName;        
    }
}

public class PetEvent
{
    public EventType EventType { get; private set; }
    Dictionary<string, HashSet<Trait>> traits;

    void AddTrait(Trait trait)
    {
        var name = trait.GetType().Name;
        if (!traits.ContainsKey(name))
            traits.Add(name, new HashSet<Trait>());
        traits[name].Add(trait);
    }

    void RemoveTrait(Trait trait)
    {
        var name = trait.GetType().Name;
        if (!traits.ContainsKey(name))
            return;
        traits[name].Remove(trait);
    }

    public bool EventActive(string traitName)
    {
        if (traits.TryGetValue(traitName, out HashSet<Trait> set))
            return set.Count > 0;
        return false;
    }

    public PetEvent(EventType eventType)
    {
        traits = new Dictionary<string, HashSet<Trait>>();
        EventType = eventType;
    }
}

public enum EventType
{
    None,
    Sight,
    Hearing,
    Smell,
    Touch,
    Internal,
    Player
}
