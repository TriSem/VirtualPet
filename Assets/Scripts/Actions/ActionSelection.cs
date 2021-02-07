using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelection : MonoBehaviour
{
    List<PetAction> petActions = null;
    ActionSet activeCandidates = new ActionSet();
    PetAction currentAction = null;
    
    void Start()
    {
        petActions = new List<PetAction>(GetComponents<PetAction>());
    }

    public void StartSelection()
    {
        foreach (var action in petActions)
            if (action.RequirementsMet())
                activeCandidates.Add(action);

        var reflexes = activeCandidates.GetByPriority(Priority.Reflex);

        if(reflexes.Count > 0)
        {
            currentAction?.Stop();
            currentAction = GetOptimalAction(reflexes);
            currentAction.Begin();
            return;
        }
        else if(currentAction?.Status != ActionStatus.Ongoing)
        {
            currentAction.Continue();
        }
        else
        {
            var regularActions = activeCandidates.GetByPriority(Priority.Regular);
            currentAction = GetOptimalAction(regularActions);
            currentAction.Begin();
        }
    }

    /// <summary>
    /// Chooses the action with the highest utility.
    /// </summary>
    /// <returns>PetAction or null if the list of actions was empty.</returns>
    PetAction GetOptimalAction(List<PetAction> actions)
    {
        PetAction best = null;
        float highestUtility = float.MinValue;
        foreach(var action in actions)
        {
            action.ChooseOptimalSetup();
            float utility = action.GetUtility();
            if(utility > highestUtility)
            {
                highestUtility = utility;
                best = action;
            }
        }
        return best;
    }
}

public class ActionSet
{
    Dictionary<Priority, List<PetAction>> actions = new Dictionary<Priority, List<PetAction>>();

    public ActionSet()
    {
        var priorities = (Priority[])Enum.GetValues(typeof(Priority));
        foreach (var priority in priorities)
            actions.Add(priority, new List<PetAction>());
    }

    public void Clear()
    {
        foreach(var entry in actions)
        {
            entry.Value.Clear();
        }
    }

    public void Add(PetAction action)
    {
        actions[action.Priority].Add(action);
    }

    public void Remove(PetAction action)
    {
        actions[action.Priority].Remove(action);
    }

    public List<PetAction> GetByPriority(Priority priority)
    {
        return actions[priority];
    }
}
