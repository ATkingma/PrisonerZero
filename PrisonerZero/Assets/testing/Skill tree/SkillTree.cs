using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public static List<SkillTree> SkillTreeList;

    public List<SkillTreeNode> SkillNodes => skillNodes;  

    [SerializeField]
    private List<SkillTreeNode> skillNodes = new List<SkillTreeNode>();

    private void Awake()
    {
        if(SkillTreeList == null)
        {
            SkillTreeList = new List<SkillTree>();
        }
        if (!SkillTreeList.Contains(this))
        {
            SkillTreeList.Add(this);
        }
    }

    public void GiveBuffs()
    {
        foreach (SkillTreeNode node in skillNodes)
        {
            Debug.Log(DataManager.Instance.upgrades);
            SkillNode upgrade = DataManager.Instance.upgrades.FirstOrDefault(u => u.skillName == node.Upgrade.skillName);
            if (upgrade != null && upgrade.isUnlocked)
            {
                node.UpgradeCall.Invoke();
            }
        }
    }

    public void Printd(string n) { print(n); }
}
