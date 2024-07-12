using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance => instance;

    private static UiManager instance;

    [SerializeField]
    private CardManager cardManager;

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


    public void UpdateXP(float currentLevel)
    {
      // Debug.Log("Current xp is now: "+ currentLevel);
    }

    public void OpenLevelUpMenu()
    {
        cardManager.gameObject.SetActive(true);
        cardManager.EnableCards();
        GameTimeManager.Instance.SetTimeScale(0);
    }

    public void CloseLevelUpMenu()
    {
        cardManager.gameObject.SetActive(false);
        cardManager.DisableCards();
        GameTimeManager.Instance.ResetTimeSCale();
    }
}
