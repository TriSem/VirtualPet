using System.Collections.Generic;
using UnityEngine;

public class ActionSelection : MonoBehaviour
{
    List<PetAction> petActions = null;

    void Start()
    {
        var actions = new List<PetAction>(GetComponents<PetAction>());    
    }

    void StartSelection()
    {
        var activeCandidates = new List<PetAction>();
        foreach (var action in petActions)
            if (action.RequirementsMet())
                activeCandidates.Add(action);


    }
}
