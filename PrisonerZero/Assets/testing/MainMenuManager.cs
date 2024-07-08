using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPanel;

    [SerializeField]
    private GameObject characterPanel;

    [SerializeField]
    private GameObject weaponPanel;

    [SerializeField]
    private GameObject selectGamePanel;

    private void Start()
    {
        SwitchPanel("main");
    }

    public void ResetAll()
    {
        mainPanel.SetActive(false);
        characterPanel.SetActive(false);
        weaponPanel.SetActive(false);
        selectGamePanel.SetActive(false);
    }

    private void EnableMainPanel()
    {
        ResetAll();
        mainPanel.SetActive(true);
    }

    private void EnableCharacterPanel()
    {
        ResetAll();
        characterPanel.SetActive(true);
    }

    private void EnableWeaponPanel()
    {
        ResetAll();
        weaponPanel.SetActive(true);
    }

    private void EnableSelectGamePanel()
    {
        ResetAll();
        selectGamePanel.SetActive(true);
    }

    public void SwitchPanel(string panelName)
    {
        ResetAll();
        switch (panelName)
        {
            case "main":
                EnableMainPanel();
                break;
            case "character":
                EnableCharacterPanel();
                break;
            case "weapon":
                EnableWeaponPanel();
                break;
            case "selectGame":
                EnableSelectGamePanel();
                break;
            case "reset":
                ResetAll();
                break;
            default:
                Debug.LogWarning("Panel not found: " + panelName);
                break;
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
