using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpManager : MonoBehaviour
{
    public static XpManager Instance => instance;

    private static XpManager instance;

    private float currentXp = 0;

    private int currentLevel = 1;

    private float xpRequired;


    [SerializeField]
    private float baseXp = 100f;

    [SerializeField]
    private float growthRate = 0.2f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        xpRequired = CalculateXpRequired(currentLevel);
    }


    public void AddXp(float xpToAdd)
    {
        currentXp += xpToAdd;
        UiManager.Instance.UpdateXP(currentXp);
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (currentXp >= xpRequired)
        {
            currentXp -= xpRequired;
            currentLevel++;
            xpRequired = CalculateXpRequired(currentLevel);
            UiManager.Instance.OpenLevelUpMenu();
        }
    }

    private float CalculateXpRequired(int level)
    {
        return baseXp * Mathf.Pow(1 + growthRate, level - 1);
    }
}
