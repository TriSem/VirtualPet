using UnityEngine;
using System;

[RequireComponent(typeof(WorldObject))]
public abstract class Trait : MonoBehaviour
{
    public WorldObject AssociatedObject { get; protected set; } = default;

    public uint TraitId { get; }
}