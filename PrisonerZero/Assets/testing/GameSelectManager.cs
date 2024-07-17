using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject NormalPanel;

    [SerializeField]
    private GameObject hardPanel;

    [SerializeField]
    private GameObject ExtreemPanel;

    private void Start()
    {
        SwitchPanel("normal");
    }

    private void ResetPanels()
    {
        NormalPanel.SetActive(false);
        hardPanel.SetActive(false);
        ExtreemPanel.SetActive(false);
    }
    private void EnableNormal()
    {
        ResetPanels();
        NormalPanel.SetActive(true);
    }

    private void EnableHardPanel()
    {
        ResetPanels();
        hardPanel.SetActive(true);
    }

    private void EnableExtreemPanel()
    {
        ResetPanels();
        ExtreemPanel.SetActive(true);
    }

    public void SwitchPanel(string panelName)
    {
        switch(panelName)
        {
            case "normal":
                EnableNormal();
                GameManager.Instance.SetDifficulty(Difficulty.Normal);
                break;
            case "hard":
                EnableHardPanel();
                GameManager.Instance.SetDifficulty(Difficulty.Hard);
                break;
            case "extreem":
                EnableExtreemPanel();
                GameManager.Instance.SetDifficulty(Difficulty.Extreem);
                break;
            default:
                Debug.LogWarning("Panel not found: " + panelName);
                SwitchPanel("normal");
                break;
        }
    }
}
