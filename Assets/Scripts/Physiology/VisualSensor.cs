using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VisualSensor : MonoBehaviour
{
    [SerializeField] Perception perception = null;

    new Collider collider = null;

    void Start()
    {
        collider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        var worldObject = other.GetComponent<WorldObject>();
        if(worldObject.Visibility >= perception.VisibilityThreshold)
            perception.Add(PerceptionType.Visual, worldObject);
    }

    void OnTriggerExit(Collider other)
    {
        var worldObject = other.GetComponent<WorldObject>();
        perception.Remove(PerceptionType.Visual, worldObject);
    }
}
