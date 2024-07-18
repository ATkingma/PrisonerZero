using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => instance;
    private static GameManager instance;

    private Difficulty currentDifficulty;

    private int currentLevel;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;

        currentLevel = 1;
    }   

    public void SetDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
    }

    public void BossBeaten()
    {
        SetLevel(currentLevel+1);
        if (currentLevel == 10)
        {
            for (int i = 0; i<50;i++)
            {
                Debug.Log("Negers");
            }
            SceneManager.LoadScene(1);
        }
        else
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}