using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public PlayerData()
    {
        valuta = new Valuta(69,0,0,0,0,0,0,0);
        playerSettings = new PlayerSettings();
        skillTree = new SkillTreeData();
    }

    public Valuta Valuta => valuta;
    [SerializeField]
    private Valuta valuta;
    public PlayerSettings PlayerSettings => playerSettings;
    [SerializeField]
    private PlayerSettings playerSettings;
    public SkillTreeData SkillTree => skillTree;
    [SerializeField]
    private SkillTreeData skillTree;

    public void SetValuta(Valuta newValuta)
    {
        valuta = newValuta; 
    }
}