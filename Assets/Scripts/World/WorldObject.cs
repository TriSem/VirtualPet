using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField] float visibility = 1f;
    [SerializeField] float loudness = 0f;
    [SerializeField] float smelliness = 0f;

    public float Visibility => visibility;
    public float Loudness => loudness;
    public float Smelliness => smelliness;

    public List<Trait> Traits { get; private set; }

    void Start()
    {
        Traits = new List<Trait>(GetComponents<Trait>());
    }
}
