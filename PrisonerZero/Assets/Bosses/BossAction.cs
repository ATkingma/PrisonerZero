using UnityEngine;

[CreateAssetMenu(fileName = "Boss_Action", menuName = "Bosses/Boss_Aciton")]
public class BossAction : ScriptableObject
{
    public BossAttackType AttackType;
    public int projectileAmount = 1;
    public float projectileSpread = 10;
    public float projectileSpeed = 1;

    public float timeBetweenWaves = 1;
    public int waveAmount = 1;
}

public enum BossAttackType{
    Projectiles,
    Wave,
    Melee,
}
