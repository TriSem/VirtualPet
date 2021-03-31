using System.Collections.Generic;
using UnityEngine;

// Gives all commands the player issues to any pets that might
// be listening.
[CreateAssetMenu(fileName = "CommandHub", menuName = "ScriptableObjects/CommandHub")]
public class CommandHub : ScriptableObject
{
    List<ICommandReceiver> commandRecievers = new List<ICommandReceiver>();

    public void Register(ICommandReceiver commandReceiver)
    {
        commandRecievers.Add(commandReceiver);
    }

    public void Unregister(ICommandReceiver CommandReciever)
    {
        commandRecievers.Remove(CommandReciever);
    }

    public void PushSignal(string command)
    {
        foreach(var reciever in commandRecievers)
        {
            reciever.RecieveCommand(command);
        }
    }
}

public interface ICommandReceiver
{
    void RecieveCommand(string command);
}