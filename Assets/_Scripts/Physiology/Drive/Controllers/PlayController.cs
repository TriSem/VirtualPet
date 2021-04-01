using UnityEngine;

public class PlayController : MonoBehaviour
{
    [SerializeField] PetAgent agent;
    [SerializeField] float flatPlayValue;

    void Start()
    {
        agent.DriveVector.SetValue(Drive.Play, flatPlayValue);
    }
}
