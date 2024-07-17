using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance => instance;
    private static SceneHandler instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
    }

    private void Start()
    {
        foreach (SkillTree skilltree in SkillTree.SkillTreeList)
        {
            skilltree.GiveBuffs();
        }
    }

    public void PlayerDied()
    {
        for (int i = 0; i < SkillTree.SkillTreeList.Count; i++)
        {
            SkillTree.SkillTreeList[i].ClearSkillTree();
        }
    }
}
