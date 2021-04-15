using UnityEngine;

public class CommandButton : MonoBehaviour
{
    [SerializeField] 
    string command = default;

    [SerializeField] 
    CommandHub audioHub = default;

    public void OnSendCommand()
    {
        audioHub.PushSignal(command);
    }
}
