using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private int sceneIndex;
    private void Start()
    {
        SceneManager.LoadScene(sceneIndex); 
    }
}
