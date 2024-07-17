using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillTreeData
{
    public List<SkillNode> Upgrades => upgrades;
    [SerializeField] 
    private List<SkillNode> upgrades;

    public SkillTreeData()
    {
        upgrades = new List<SkillNode>();
    }
}
