using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioHub", menuName = "ScriptableObjects/AudioHub")]
public class AudioHub : ScriptableObject
{
    List<IAudioReciever> audioRecievers = new List<IAudioReciever>();

    public void Register(IAudioReciever audioReciever)
    {
        audioRecievers.Add(audioReciever);
    }

    public void Unregister(IAudioReciever audioReciever)
    {
        audioRecievers.Remove(audioReciever);
    }

    public void PushSignal(Command command)
    {
        foreach(var reciever in audioRecievers)
        {
            reciever.RecieveSignal(command);
        }
    }
}

public interface IAudioReciever
{
    void RecieveSignal(Command command);
}