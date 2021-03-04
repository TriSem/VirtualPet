using UnityEngine;

public abstract class Reaction : MonoBehaviour, IBehaviour
{
    public ActionStatus Status { get; protected set; }

    public abstract void Cancel();
    public abstract void Use(PetAgent agent);
    public abstract bool CanDoBoth(IBehaviour other);

    public bool Triggered { get; set; } = false;
}
