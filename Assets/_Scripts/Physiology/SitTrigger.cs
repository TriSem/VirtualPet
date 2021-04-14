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
            agent.BehaviourSelection.GiveBonus("Sit", Mathf.Max(agent.DriveVector.GetValue(Drive.Food) * 3, 0f));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagName))
        {
            Triggered = false;
            agent.BehaviourSelection.RemoveBonus("Sit");
        }
    }
}
