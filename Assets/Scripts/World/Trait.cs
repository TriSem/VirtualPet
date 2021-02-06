using UnityEngine;

[RequireComponent(typeof(WorldObject))]
public class Trait : MonoBehaviour
{
    public WorldObject AssociatedObject { get; protected set; } = default;
    public int TraitId { get; protected set; } = 0;
}