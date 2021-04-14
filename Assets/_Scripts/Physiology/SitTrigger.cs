using UnityEngine;

public class SitTrigger : MonoBehaviour
{
    [SerializeField] string tagName = default;
    [SerializeField] PetAgent agent = null;
    [SerializeField] float duration = 2f;

    float removeBonusTime = 0f;

    public bool Triggered { get; private set; }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagName))
        {
            Triggered = true;
            agent.BehaviourSelection.GiveBonus("Sit", Mathf.Max(agent.DriveVector.GetValue(Drive.Food) * 3, 0f));
            removeBonusTime = Time.time + duration;
        }
    }

    void Update()
    {
        if(Triggered && Time.time > removeBonusTime)
        {
            Triggered = false;
            agent.BehaviourSelection.RemoveBonus("Sit");
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag(tagName))
    //    {
    //        Triggered = false;
    //        agent.BehaviourSelection.RemoveBonus("Sit");
    //    }
    //}
}
