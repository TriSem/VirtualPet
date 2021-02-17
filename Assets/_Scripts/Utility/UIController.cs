using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject drivePanel = default;
    [SerializeField] GameObject commandPanel = default;

    bool showDrives = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            showDrives = !showDrives;
            drivePanel.SetActive(showDrives);
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
