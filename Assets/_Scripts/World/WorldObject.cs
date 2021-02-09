using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField] string objectName = "Unknown";
    [SerializeField] float visibility = 1f;
    [SerializeField] float audibility = 0f;
    [SerializeField] float smelliness = 0f;

    [SerializeField, Tooltip("Mark object to be commited to pets memory.")] 
    bool isStatic = false;

    /// <summary>
    /// Determines wether an object will be put into the pets long term
    /// memory or not.
    /// </summary>
    public bool IsStatic => isStatic;

    public string Name => objectName;
    public float Visibility => visibility;
    public float Audibility => audibility;
    public float Smelliness => smelliness;

    public List<ActionObject> Actions { get; private set; }

    void Start()
    {
        Actions = new List<ActionObject>(GetComponents<ActionObject>());
    }
}
