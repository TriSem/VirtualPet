﻿using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class WorldObject : MonoBehaviour
{
    [SerializeField] string objectName = "Unknown";
    [SerializeField] float visibility = 1f;
    [SerializeField] float audibility = 0f;
    [SerializeField] float smelliness = 0f;
    [SerializeField] bool memorable = false;

    new Rigidbody rigidbody;

    public string Name => objectName;
    public float Visibility => visibility;
    public float Audibility => audibility;
    public float Smelliness => smelliness;
    public bool Memorable => memorable;

    public Vector3 Velocity => rigidbody == null ? Vector3.zero : rigidbody.velocity;

    public List<Behavior> Behaviors { get; private set; }

    void Awake() => WorldBlackboard.Instance.Add(this);    

    void Start()
    {
        Behaviors = new List<Behavior>(GetComponents<Behavior>());
        rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable() => WorldBlackboard.Instance.Add(this);

    void OnDestroy() => WorldBlackboard.Instance.Remove(this);

    void OnDisable() => WorldBlackboard.Instance.Remove(this);
}

public sealed class WorldBlackboard
{
    static readonly Lazy<WorldBlackboard> lazy = new Lazy<WorldBlackboard>(() => new WorldBlackboard());

    HashSet<WorldObject> worldObjects = new HashSet<WorldObject>();

    public static WorldBlackboard Instance => lazy.Value; 

    private WorldBlackboard() { }

    public void Add(WorldObject worldObject) => worldObjects.Add(worldObject);

    public void Remove(WorldObject worldObject) => worldObjects.Remove(worldObject);

    public HashSet<WorldObject> GetObjects() => new HashSet<WorldObject>(worldObjects);
}
