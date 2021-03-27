using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(BehaviourSelection))]
public class PetAgent : MonoBehaviour
{
    [SerializeField] float tickRate = (1 / 10);
    [SerializeField] MotorSystem motorSystem = null;
    [SerializeField] Perception perception = null;
    [SerializeField] Stomach stomach = null;
    [SerializeField] Grasp snoot = null;
    BehaviourSelection actionSelection = null;

    public BoxCollider BoundingBox { get; private set; } = null;

    public Stomach Stomach => stomach;
    public Perception Perception => perception;
    public DriveVector DriveVector { get; } = new DriveVector();
    public MotorSystem Motor => motorSystem;
    public Grasp Snoot => snoot;

    float lastTick = (1 / 5);

    void Awake()
    {
        BoundingBox = GetComponent<BoxCollider>();
        actionSelection = GetComponent<BehaviourSelection>();
    }

    void Update()
    {
        float time = Time.time;
        if (lastTick + tickRate < time)
        {
            lastTick = time;
            actionSelection.EvaluateActions();
        }
    }
}
