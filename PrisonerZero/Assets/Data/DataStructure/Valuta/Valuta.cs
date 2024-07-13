using System;

[Serializable]
public class Valuta
{
    public int SoulFragements { get; private set; }
    public int DevineTokens { get; private set; }
    public int AscensionTokens { get; private set; }
    public int PrestigeTokens { get; private set; }
    public int ValorTokens { get; private set; }
    public int EtherealTokens { get; private set; }
    public int PhoenixTokens { get; private set; }
    public int InfinityTokens { get; private set; }

    public Valuta(int soulFragments, int devineTokens, int ascensionTokens,int prestigeTokens, int valorTokens, int etherealTokens,int phoenixTokens, int infinityTokens)
    {
        SoulFragements = soulFragments;
        DevineTokens = devineTokens;
        AscensionTokens = ascensionTokens;
        PrestigeTokens = prestigeTokens;
        ValorTokens = valorTokens;
        EtherealTokens = etherealTokens;
        PhoenixTokens = phoenixTokens;
        InfinityTokens = infinityTokens;
    }
}