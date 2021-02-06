using System.Collections.Generic;
using UnityEngine;
using System;

public sealed partial class WorldObject : MonoBehaviour
{
    [SerializeField] string ingameName = "object";

    static ulong typeId = 0;

    public ulong InstanceId { get; private set; }
    public ulong TypeId => typeId;

    public string Name => ingameName;

    public Dictionary<Type, Trait> Traits { get; } = new Dictionary<Type, Trait>();

    void Awake()
    {
        InstanceId = WorldDatabase.Get.AddWithKey(this);
        if(typeId == 0)
            typeId = IdGeneration.Djb2(ingameName);
        var traitList = GetComponents<Trait>();
        foreach (var trait in traitList)
            Traits.Add(trait.GetType(), trait);
    }

    public bool HasTrait<T>() where T : Trait => Traits.ContainsKey(typeof(T));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool HasTrait<T>(out T result) where T : Trait
    {
        result = default;
        if(Traits.TryGetValue(typeof(T), out Trait trait))
        {
            result = trait as T;
            return true;
        }
        return false;
    }
}
