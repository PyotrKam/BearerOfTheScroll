using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string levelSceneName; 
    [SerializeField] private int defaultLives = 3;  

    public void StartLevel()
    {        
        FallManager.StartNewRun(defaultLives);
        SceneManager.LoadScene(levelSceneName, LoadSceneMode.Single);
    }
}
