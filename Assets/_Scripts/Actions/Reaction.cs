using UnityEngine;

public abstract class Reaction : MonoBehaviour, IAction
{
    public ActionStatus Status { get; protected set; }

    public abstract void Cancel();
    public abstract void UseAction(PetAgent agent);

    public bool Triggered { get; set; } = false;
}
