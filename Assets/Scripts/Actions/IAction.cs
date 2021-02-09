using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    ActionStatus Status { get; }
    void UseAction(PetAgent agent);
    void Cancel();
}
