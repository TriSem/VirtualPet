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

    public Dictionary<uint, Trait> Traits { get; } = new Dictionary<uint, Trait>();

    void Awake()
    {
        InstanceId = WorldDatabase.Get.AddWithKey(this);
        if(typeId == 0)
            typeId = IdGeneration.Djb2(ingameName);
        var traitList = GetComponents<Trait>();
        foreach (var trait in traitList)
            Traits.Add(trait.TraitId, trait);
    }

    public bool HasTrait(uint traitId) => Traits.ContainsKey(traitId);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public Trait GetTrait(uint traitId)
    {
        if(Traits.TryGetValue(traitId, out Trait trait))
        {
            return trait;
        }
        return null;
    }
}
