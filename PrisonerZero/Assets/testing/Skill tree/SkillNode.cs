using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SkillNode", menuName = "Skill Tree/Skill Node")]
[Serializable]
public class SkillNode : ScriptableObject
{
    public string skillName;
    public int cost;
    public Valutas myValuta;
    public bool isUnlocked;
    public List<SkillNode> dependencies;

    public SkillNode(string name,int cost,Valutas val, bool unlocked)
    {
        skillName = name;
        this.cost = cost;
        this.myValuta = val;
        this.isUnlocked = unlocked; 
        dependencies = new List<SkillNode>();
    }
}

[Serializable]
public class SkillTreeNode
{
    [SerializeField]
    private SkillNode upgrade;

    public SkillNode Upgrade => upgrade;

    [SerializeField]
    private List<GameObject> lines;

    public List<GameObject> Lines => lines;

    [SerializeField]
    private Button myButton;

    public Button MyButton => myButton;

    [SerializeField]
    private UnityEvent upgradeCall;

    public UnityEvent UpgradeCall => upgradeCall;
}