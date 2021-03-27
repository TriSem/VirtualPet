using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sensor : MonoBehaviour
{
    [SerializeField] protected Modalities modalities;

    // Determines which kinds of signals the sensor can pick up.
    public Modalities Modalities => modalities;

    public abstract HashSet<WorldObject> GetPercievedObjects();

}
