using System.Collections.Generic;
using UnityEngine;

public class ActionSelection : MonoBehaviour
{
    [SerializeField] PetAgent agent;
    IAction currentAction = null;

    public void SelectAction()
    {
        var reaction = CheckForReactions();
        if(reaction != null)
        {
            currentAction.Cancel();
            currentAction = reaction;
            currentAction.UseAction(agent);
        }
        else if(currentAction.Status != ActionStatus.Ongoing)
        {
            HashSet<WorldObject> percievedObjects = agent.Perception.GetWorldObjects();
            ActionObject mostUseful = MostUsefulAction(percievedObjects);
            currentAction = mostUseful;
            currentAction.UseAction(agent);
        }
    }

    public ActionObject CheckForReactions()
    {
        // TODO: Add support for reactions.
        return null;
    }

    public ActionObject MostUsefulAction(HashSet<WorldObject> worldObjects)
    {
        ActionObject mostUseful = null;
        float highestUtility = float.MinValue;

        foreach (var worldObject in worldObjects)
        {
            foreach (var action in worldObject.Actions)
            {
                if (action.IsUsable())
                {
                    float utility = action.CalculateUtility(agent.DriveVector);
                    if (utility > highestUtility)
                    {
                        mostUseful = action;
                        highestUtility = utility;
                    }
                }
            }
        }

        return mostUseful;
    }
}
