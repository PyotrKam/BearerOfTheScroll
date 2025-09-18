using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelEndUI : MonoBehaviour
{    
    public static string FinishMessage = "Finish!";

    [Header("UI References")]
    [SerializeField] private Text messageText;       
    [SerializeField] private Button nextLevelButton;     
    [SerializeField] private Button toMenuButton;       

    [Header("Menu Scene")]
    [SerializeField] private string menuSceneName = "LobbyUI"; 

    private void Awake()
    {
        
        if (messageText != null)
            messageText.text = string.IsNullOrEmpty(FinishMessage) ? "Finish!" : FinishMessage;

        
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(LoadNextLevel);

        if (toMenuButton != null)
            toMenuButton.onClick.AddListener(LoadMenu);

        
        var limiter = FindObjectOfType<MovementLimiter>();
        limiter?.DisableMovement();
    }

    private void LoadNextLevel()
    {
        
        string uiSceneName = gameObject.scene.name;
        Scene gameScene = default;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var s = SceneManager.GetSceneAt(i);
            if (s.name != uiSceneName)
            {
                gameScene = s;
                break;
            }
        }

        int currentIndex = gameScene.buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            
            SceneManager.LoadScene(nextIndex, LoadSceneMode.Single);
        }
        else
        {
            
            LoadMenu();
        }
    }

    private void LoadMenu()
    {
        if (!string.IsNullOrEmpty(menuSceneName))
        {
            SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogWarning("[LevelEndUI] Unknow name scene");
        }
    }
}
