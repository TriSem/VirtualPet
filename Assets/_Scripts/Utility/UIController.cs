using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject drivePanel = default;
    [SerializeField] GameObject commandPanel = default;
    [SerializeField] GameObject statePanel = default;

    bool debugHelpOn = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ToggleDebugHelp();
    }

    void ToggleDebugHelp()
    {
        drivePanel.SetActive(debugHelpOn);
        statePanel.SetActive(debugHelpOn);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            debugHelpOn = !debugHelpOn;
            ToggleDebugHelp();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            commandPanel.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.C))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            commandPanel.SetActive(false);
        }
    }
}
