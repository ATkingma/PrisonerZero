using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance => instance;
    private static SceneHandler instance;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private DungeonGenerator generator;


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

        player.SetActive(false);
        player.transform.position = Vector3.zero;

        generator.StartGeneration();
    }

    public void PlayerDied()
    {
        for (int i = 0; i < SkillTree.SkillTreeList.Count; i++)
        {
            SkillTree.SkillTreeList[i].ClearSkillTree();
        }
    }

    public void EnablePlayer()
    {
        player.SetActive (true);
    }
}
