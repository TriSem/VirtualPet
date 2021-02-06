using UnityEngine;

[RequireComponent(typeof(WorldObject))]
public abstract class Trait : MonoBehaviour
{
    public abstract uint TraitId { get; }
}