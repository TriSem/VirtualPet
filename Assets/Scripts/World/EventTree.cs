using System;
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
        throw new NotImplementedException();
    }
}

public class EventQuery
{
    public Trait Trait { get; set; }
    public EventType EventType { get; private set; }

    public EventQuery(EventType type, Trait trait)
    {
        Trait = trait;
        EventType = type;
    }
}

public class PetEvent
{
    public EventType EventType { get; private set; }
    Dictionary<Type, HashSet<Trait>> traits;

    void AddTrait(Trait trait)
    {
        var type = trait.GetType();
        if (!traits.ContainsKey(type))
            traits.Add(type, new HashSet<Trait>());
        traits[trait.GetType()].Add(trait);
    }

    void RemoveTrait(Trait trait)
    {
        var type = trait.GetType();
        if (!traits.ContainsKey(type))
            return;
        traits[type].Remove(trait);
    }

    bool ContainsElements<T>(HashSet<Trait> set) where T : Trait
    {
        if (traits.TryGetValue(typeof(T), out HashSet<Trait> result))
            return set.IsSubsetOf(result);
        return false;
    }

    public PetEvent(EventType eventType)
    {
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
