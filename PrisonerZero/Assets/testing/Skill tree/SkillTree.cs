using System;
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

        foreach (SkillTreeNode node in SkillNodes)
        {
            if (DataManager.Instance.upgrades.Contains(node.Upgrade) && node.Upgrade.isUnlocked)
            {
                foreach (GameObject line in node.Lines)
                {
                    line.SetActive(true);
                }
            }
        }
    }

    private void Start()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        foreach (SkillTreeNode node in skillNodes)
        {
            if (node.MyButton != null)
            {
                node.MyButton.onClick.AddListener(() => NodePressed(node));
            }
        }
    }
    
    public void GiveBuffs()
    {
        foreach (SkillTreeNode node in skillNodes)
        {
            if (node.Upgrade.isUnlocked)
            {
                node.UpgradeCall.Invoke();
            }
        }
    }

    public void ClearSkillTree()
    {
        SkillTreeList.Remove(this);
        Destroy(gameObject);
    }


    private void NodePressed(SkillTreeNode skillTreeNode)
    {
        if (skillTreeNode.Upgrade.isUnlocked)
        {
            return;
        }
        int dependecyUnlockCount = 0;

        for (int i = 0; i < skillTreeNode.Upgrade.dependencies.Count; i++)
        {
            if (skillTreeNode.Upgrade.dependencies[i].isUnlocked)
            {
                dependecyUnlockCount++;
            }
        }
        if (dependecyUnlockCount == skillTreeNode.Upgrade.dependencies.Count)
        {
            UnlockWithValuta(skillTreeNode);
        }
        else
        {
            Debug.LogWarning("Unlock dependecy's first");
        }

    }

    private void UnlockWithValuta(SkillTreeNode skillTreeNode)
    {
        switch (skillTreeNode.Upgrade.myValuta)
        {
            case Valutas.SoulFragments:
                UnlockWithSoulFragments(skillTreeNode);
                break;
            case Valutas.DevineTokens:
                UnlockWithDevineTokens(skillTreeNode);
                break;
            case Valutas.AscensionTokens:
                UnlockWithAscensionTokens(skillTreeNode);
                break;
            case Valutas.PrestigeTokens:
                UnlockWithPrestigeTokens(skillTreeNode);
                break;
            case Valutas.ValorTokens:
                UnlockWithValorTokens(skillTreeNode);
                break;
            case Valutas.EtherealTokens:
                UnlockWithEtherealTokens(skillTreeNode);
                break;
            case Valutas.PhoenixTokens:
                UnlockWithPhoenixTokens(skillTreeNode);
                break;
            case Valutas.InfinityTokens:
                UnlockWithInfinityTokens(skillTreeNode);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UnlockWithSoulFragments(SkillTreeNode skillTreeNode)
    {
        if (DataManager.Instance.Valuta.SoulFragments >= skillTreeNode.Upgrade.cost)
        {
            Valuta tempValuta = DataManager.Instance.Valuta;
            tempValuta.SetValuta(skillTreeNode.Upgrade.myValuta, DataManager.Instance.Valuta.SoulFragments - skillTreeNode.Upgrade.cost);
            DataManager.Instance.SetValuta(tempValuta);
            skillTreeNode.Upgrade.isUnlocked = true;
            DataManager.Instance.upgrades.Add(skillTreeNode.Upgrade);
            foreach (GameObject line in skillTreeNode.Lines)
            {
                line.SetActive(true);
            }
            DataManager.Instance.SavePlayerData();
        }
        else
        {
            Debug.LogWarning("Not enough valuta.");
        }
    }

    private void UnlockWithDevineTokens(SkillTreeNode skillTreeNode)
    {
        if (DataManager.Instance.Valuta.DevineTokens >= skillTreeNode.Upgrade.cost)
        {
            Valuta tempValuta = DataManager.Instance.Valuta;
            tempValuta.SetValuta(skillTreeNode.Upgrade.myValuta, DataManager.Instance.Valuta.SoulFragments - skillTreeNode.Upgrade.cost);
            DataManager.Instance.SetValuta(tempValuta);
            skillTreeNode.Upgrade.isUnlocked = true;
            DataManager.Instance.upgrades.Add(skillTreeNode.Upgrade);
            foreach (GameObject line in skillTreeNode.Lines)
            {
                line.SetActive(true);
            }
            DataManager.Instance.SavePlayerData();
        }
        else
        {
            Debug.LogWarning("Not enough valuta.");
        }
    }

    private void UnlockWithAscensionTokens(SkillTreeNode skillTreeNode)
    {
        if (DataManager.Instance.Valuta.AscensionTokens >= skillTreeNode.Upgrade.cost)
        {
            Valuta tempValuta = DataManager.Instance.Valuta;
            tempValuta.SetValuta(skillTreeNode.Upgrade.myValuta, DataManager.Instance.Valuta.SoulFragments - skillTreeNode.Upgrade.cost);
            DataManager.Instance.SetValuta(tempValuta);
            skillTreeNode.Upgrade.isUnlocked = true;
            DataManager.Instance.upgrades.Add(skillTreeNode.Upgrade);
            foreach (GameObject line in skillTreeNode.Lines)
            {
                line.SetActive(true);
            }
            DataManager.Instance.SavePlayerData();
        }
        else
        {
            Debug.LogWarning("Not enough valuta.");
        }
    }

    private void UnlockWithPrestigeTokens(SkillTreeNode skillTreeNode)
    {
        if (DataManager.Instance.Valuta.PrestigeTokens >= skillTreeNode.Upgrade.cost)
        {
            Valuta tempValuta = DataManager.Instance.Valuta;
            tempValuta.SetValuta(skillTreeNode.Upgrade.myValuta, DataManager.Instance.Valuta.SoulFragments - skillTreeNode.Upgrade.cost);
            DataManager.Instance.SetValuta(tempValuta);
            skillTreeNode.Upgrade.isUnlocked = true;
            DataManager.Instance.upgrades.Add(skillTreeNode.Upgrade);
            foreach (GameObject line in skillTreeNode.Lines)
            {
                line.SetActive(true);
            }
            DataManager.Instance.SavePlayerData();
        }
        else
        {
            Debug.LogWarning("Not enough valuta.");
        }
    }

    private void UnlockWithValorTokens(SkillTreeNode skillTreeNode)
    {
        if (DataManager.Instance.Valuta.ValorTokens >= skillTreeNode.Upgrade.cost)
        {
            Valuta tempValuta = DataManager.Instance.Valuta;
            tempValuta.SetValuta(skillTreeNode.Upgrade.myValuta, DataManager.Instance.Valuta.SoulFragments - skillTreeNode.Upgrade.cost);
            DataManager.Instance.SetValuta(tempValuta);
            skillTreeNode.Upgrade.isUnlocked = true;
            DataManager.Instance.upgrades.Add(skillTreeNode.Upgrade);
            foreach (GameObject line in skillTreeNode.Lines)
            {
                line.SetActive(true);
            }
            DataManager.Instance.SavePlayerData();
        }
        else
        {
            Debug.LogWarning("Not enough valuta.");
        }
    }

    private void UnlockWithEtherealTokens(SkillTreeNode skillTreeNode)
    {
        if (DataManager.Instance.Valuta.EtherealTokens >= skillTreeNode.Upgrade.cost)
        {
            Valuta tempValuta = DataManager.Instance.Valuta;
            tempValuta.SetValuta(skillTreeNode.Upgrade.myValuta, DataManager.Instance.Valuta.SoulFragments - skillTreeNode.Upgrade.cost);
            DataManager.Instance.SetValuta(tempValuta);
            skillTreeNode.Upgrade.isUnlocked = true;
            DataManager.Instance.upgrades.Add(skillTreeNode.Upgrade);
            foreach (GameObject line in skillTreeNode.Lines)
            {
                line.SetActive(true);
            }
            DataManager.Instance.SavePlayerData();
        }
        else
        {
            Debug.LogWarning("Not enough valuta.");
        }
    }

    private void UnlockWithPhoenixTokens(SkillTreeNode skillTreeNode)
    {
        if (DataManager.Instance.Valuta.PhoenixTokens >= skillTreeNode.Upgrade.cost)
        {
            Valuta tempValuta = DataManager.Instance.Valuta;
            tempValuta.SetValuta(skillTreeNode.Upgrade.myValuta, DataManager.Instance.Valuta.SoulFragments - skillTreeNode.Upgrade.cost);
            DataManager.Instance.SetValuta(tempValuta);
            skillTreeNode.Upgrade.isUnlocked = true;
            DataManager.Instance.upgrades.Add(skillTreeNode.Upgrade);
            foreach (GameObject line in skillTreeNode.Lines)
            {
                line.SetActive(true);
            }
            DataManager.Instance.SavePlayerData();
        }
        else
        {
            Debug.LogWarning("Not enough valuta.");
        }
    }

    private void UnlockWithInfinityTokens(SkillTreeNode skillTreeNode)
    {
        if (DataManager.Instance.Valuta.InfinityTokens >= skillTreeNode.Upgrade.cost)
        {
            Valuta tempValuta = DataManager.Instance.Valuta;
            tempValuta.SetValuta(skillTreeNode.Upgrade.myValuta, DataManager.Instance.Valuta.SoulFragments - skillTreeNode.Upgrade.cost);
            DataManager.Instance.SetValuta(tempValuta);
            skillTreeNode.Upgrade.isUnlocked = true;
            DataManager.Instance.upgrades.Add(skillTreeNode.Upgrade);
            foreach (GameObject line in skillTreeNode.Lines)
            {
                line.SetActive(true);
            }
            DataManager.Instance.SavePlayerData();
        }
        else
        {
            Debug.LogWarning("Not enough valuta.");
        }
    }

    public void Printd(string n) { print(n); }
}
