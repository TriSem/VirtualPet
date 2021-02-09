using UnityEngine;
using UnityEngine.UI;

public class CurrentActionUI : MonoBehaviour
{
    [SerializeField] ActionSelection actionSelection = default;
    Text text = default;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if(actionSelection.CurrentAction == null)
        {
            text.text = "No Action";
        }
        else
        {
            string currentAction = actionSelection.CurrentAction.ToString();
            text.text = currentAction + ": " + actionSelection.CurrentUtility.ToString();
        }
    }
}
