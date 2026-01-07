using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class FallManager : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField] private int livesPerLevel = 3;

    private static int s_currentLives = -1;

    public int Lives { get; private set; }

    [Header("FX")]
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioClip fallClip;
    

    [Header("Delays")]
    [Tooltip("Pause after fall")]
    [SerializeField] private float fallPause = 0.35f;
    [Tooltip("Add time to restart level")]
    [SerializeField] private float restartDelay = 2f;

    [Header("Game Flow")]
    [Tooltip("Scene name where to go")]
    [SerializeField] private string lobbySceneName = "LobbyUI";

    public event Action<int> OnLivesChanged; // HUD
    public event Action OnFall;              // animation
    public event Action OnAfterFall;         // restart or something else

    private bool _falling;

    void Awake()
    {
        _falling = false;
        EnsureInitialized(livesPerLevel);
        Lives = s_currentLives;
        OnLivesChanged?.Invoke(Lives);
    }

    public static void StartNewRun(int defaultLives)
    {
        s_currentLives = Mathf.Max(0, defaultLives);
    }

    public static void EnsureInitialized(int defaultLives)
    {
        if (s_currentLives < 0)
            s_currentLives = Mathf.Max(0, defaultLives);
    }

    public void ResetLives()
    {
        Lives = livesPerLevel;
        OnLivesChanged?.Invoke(Lives);
    }

    public void Fall()
    {
        if (_falling) return;  
        _falling = true;

        StartCoroutine(FallRoutine());
    }

    private IEnumerator FallRoutine()
    {
        Debug.Log("[FallManager] FallRoutine start");
        sfx?.PlayOneShot(fallClip);
        OnFall?.Invoke();

        Lives = Mathf.Max(0, Lives - 1);
        s_currentLives = Lives;
        Debug.Log($"[FallManager] Lives now: {Lives}");
        OnLivesChanged?.Invoke(Lives);

        yield return new WaitForSeconds(fallPause);

        if (Lives > 0)
        {
            OnAfterFall?.Invoke();
            if (restartDelay > 0f)
                yield return new WaitForSeconds(restartDelay);

            var sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            yield break; 
        }
        else
        {
            // No Lives
            SceneManager.LoadScene(lobbySceneName, LoadSceneMode.Single);
            yield break;
        }

        /*
        if (Lives <= 0)
        {

            var sceneName = SceneManager.GetActiveScene().name;
            //Debug.Log($"[FallManager] Reload scene: {sceneName} (index {scene.buildIndex})");
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            yield break;
        }
        Debug.Log("[FallManager] OnAfterFall()");
        
        */
    }
}
