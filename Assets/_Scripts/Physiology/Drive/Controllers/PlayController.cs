using UnityEngine;

public class PlayController : MonoBehaviour
{
    [SerializeField] 
    PetAgent agent = null;

    [SerializeField] 
    float flatPlayValue = default;

    void Start()
    {
        agent.DriveVector.SetValue(Drive.Play, flatPlayValue);
    }
}
