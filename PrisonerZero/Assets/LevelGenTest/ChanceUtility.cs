
using UnityEngine;

public class ChanceUtility
{
    public static bool Chance(float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0f, 100f);
        float randomValue = Random.Range(0f, 100f);
        return randomValue < percentage;
    }
}
