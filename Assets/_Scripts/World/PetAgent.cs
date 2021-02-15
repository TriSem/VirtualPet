using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(ActionSelection))]
public class PetAgent : MonoBehaviour
{
    [SerializeField] float tickRate = (1 / 10);
    [SerializeField] MotorSystem motorSystem = null;
    [SerializeField] Perception perception = null;
    [SerializeField] Stomach stomach = null;
    ActionSelection actionSelection = null;

    public BoxCollider BoundingBox { get; private set; } = null;

    public Stomach Stomach => stomach;
    public Perception Perception => perception;
    public DriveVector DriveVector { get; } = new DriveVector();
    public MotorSystem Motor => motorSystem;

    float lastTick = (1 / 10);


    void Awake()
    {
        BoundingBox = GetComponent<BoxCollider>();
        actionSelection = GetComponent<ActionSelection>();
    }

    void Update()
    {
        float time = Time.time;
        if (lastTick + tickRate < time)
        {
            lastTick = time;
            actionSelection.SelectAction();
        }
    }
}
