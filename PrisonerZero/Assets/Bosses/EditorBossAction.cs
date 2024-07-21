using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BossAction))]
public class EditorBossAction : Editor
{
    BossAction bossAction;

    public void OnEnable()
    {
        bossAction = (BossAction)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        bossAction.AttackType = (BossAttackType)EditorGUILayout.EnumPopup(bossAction.AttackType);

        switch (bossAction.AttackType)
        {
            case BossAttackType.Projectiles:
                bossAction.projectileAmount = EditorGUILayout.IntField(bossAction.projectileAmount);
                bossAction.projectileSpread = EditorGUILayout.FloatField(bossAction.projectileSpread);
                bossAction.projectileSpeed  = EditorGUILayout.FloatField(bossAction.projectileSpeed);
                break;
            case BossAttackType.Wave:
                break;
            case BossAttackType.Melee:
                break;
        }
        EditorGUI.EndChangeCheck();
        
        if (GUI.changed)
            SaveData();
    }
    

    private void SaveData()
    {
        EditorUtility.SetDirty(bossAction);
        AssetDatabase.SaveAssets();
    }
}
