using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => instance;
    private static GameManager instance;

    private Difficulty currentDifficulty;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
    }   

    public void SetDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;
    }
}