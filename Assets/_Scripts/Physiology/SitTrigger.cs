using UnityEngine;

public class SitTrigger : MonoBehaviour
{
    [SerializeField] string tagName = default;
    [SerializeField] PetAgent agent = null;

    public bool Triggered { get; private set; }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
        {
            Triggered = true;
            agent.BehaviourSelection.GiveBonus("Sit", agent.DriveVector.GetValue(Drive.Food) * 3);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Triggered = false;
        agent.BehaviourSelection.RemoveBonus("Sit");
    }
}
