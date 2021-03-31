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
        if(actionSelection.CurrentBehavior == null)
        {
            text.text = "No Action";
        }
        else
        {
            string currentAction = actionSelection.CurrentBehavior.ToString();
            text.text = currentAction + ": " + actionSelection.CurrentUtility.ToString("F2");
        }
    }
}
