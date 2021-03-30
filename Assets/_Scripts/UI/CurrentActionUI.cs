using UnityEngine;
using UnityEngine.UI;

public class CurrentActionUI : MonoBehaviour
{
    [SerializeField] BehaviourSelection actionSelection = default;
    Text text = default;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if(actionSelection.CurrentOption == null)
        {
            text.text = "No Action";
        }
        else
        {
            string currentAction = actionSelection.CurrentOption.GetCurrentBehavior().ToString();
            text.text = currentAction + ": " + actionSelection.CurrentOption.TotalUtility.ToString("F2");
        }
    }
}
