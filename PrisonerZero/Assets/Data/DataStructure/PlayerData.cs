using System;

[Serializable]
public class PlayerData
{
    public Valuta Valuta { get; private set; }
    public PlayerSettings PlayerSettings { get; private set; }
    public SkillTree SkillTree { get; private set; }
    public NoviceTree NoviceTree { get; private set; }

    public void SetValuta(Valuta newValuta)
    {
        Valuta = newValuta; 
    }
}