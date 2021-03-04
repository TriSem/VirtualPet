using System;
using System.Collections.Generic;
using UnityEngine;

public class Learning : MonoBehaviour
{
    Dictionary<string, LearnedSkill> skills = new Dictionary<string, LearnedSkill>();
}

[Serializable]
public class LearnedSkill
{
    [SerializeField] string actionName = "";
    Dictionary<string, int> phrasesHeard = new Dictionary<string, int>();

    ICondition startTrigger, endTrigger;

    public bool Active { get; set; } = false;
    public bool Learned { get; set; } = false;
}
