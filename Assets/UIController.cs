using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject drivePanel = default;
    [SerializeField] GameObject commandPanel = default;
    [SerializeField] FirstPersonAIO firstPerson = default;

    bool showDrives = false;

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
            firstPerson.enabled = false;
            commandPanel.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.C))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            firstPerson.enabled = true;
            commandPanel.SetActive(false);
        }
    }
}
