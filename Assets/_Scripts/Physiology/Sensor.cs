using System.Collections.Generic;
using UnityEngine;

public abstract class Sensor : MonoBehaviour
{
    public abstract HashSet<WorldObject> GetPercievedObjects();
}
