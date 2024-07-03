using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance => instance;

    private static GameTimeManager instance;

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

    public void SetTimeScale(float _newTimescale)
    {
        Time.timeScale = _newTimescale; 
    }

    public void ResetTimeSCale()
    {
        Time.timeScale = 1.0f;
    }
}