using System;
using UnityEngine;

[Serializable]
public class Valuta
{
    [SerializeField]
    private int soulFragments;
    [SerializeField]
    private int devineTokens;
    [SerializeField]
    private int ascensionTokens;
    [SerializeField]
    private int prestigeTokens;
    [SerializeField]
    private int valorTokens;
    [SerializeField]
    private int etherealTokens;
    [SerializeField]
    private int phoenixTokens;
    [SerializeField]
    private int infinityTokens;

    public int SoulFragments => soulFragments;
    public int DevineTokens => devineTokens;
    public int AscensionTokens => ascensionTokens;
    public int PrestigeTokens => prestigeTokens;
    public int ValorTokens => valorTokens;
    public int EtherealTokens => etherealTokens;
    public int PhoenixTokens => phoenixTokens;
    public int InfinityTokens => infinityTokens;

    public void SetValuta(Valutas valuta, int value)
    {
        switch (valuta)
        {
            case Valutas.SoulFragments:
                soulFragments = value;
                break;
            case Valutas.DevineTokens:
                devineTokens = value;
                break;
            case Valutas.AscensionTokens:
                ascensionTokens = value;
                break;
            case Valutas.PrestigeTokens:
                prestigeTokens = value;
                break;
            case Valutas.ValorTokens:
                valorTokens = value;
                break;
            case Valutas.EtherealTokens:
                etherealTokens = value;
                break;
            case Valutas.PhoenixTokens:
                phoenixTokens = value;
                break;
            case Valutas.InfinityTokens:
                infinityTokens = value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(valuta), valuta, null);
        }
    }

    public Valuta(int soulFragments, int devineTokens, int ascensionTokens, int prestigeTokens,
                  int valorTokens, int etherealTokens, int phoenixTokens, int infinityTokens)
    {
        this.soulFragments = soulFragments;
        this.devineTokens = devineTokens;
        this.ascensionTokens = ascensionTokens;
        this.prestigeTokens = prestigeTokens;
        this.valorTokens = valorTokens;
        this.etherealTokens = etherealTokens;
        this.phoenixTokens = phoenixTokens;
        this.infinityTokens = infinityTokens;
    }
}
[Serializable]
public enum Valutas
{
    SoulFragments,
    DevineTokens,
    AscensionTokens,
    PrestigeTokens,
    ValorTokens,
    EtherealTokens,
    PhoenixTokens,
    InfinityTokens,
}
