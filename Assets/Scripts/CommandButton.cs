using UnityEngine;

public class CommandButton : MonoBehaviour
{
    [SerializeField] Command command = default;
    [SerializeField] AudioHub audioHub = default;

    public void OnSendCommand()
    {
        audioHub.PushSignal(command);
    }
}
