using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameHUDLoader : MonoBehaviour
{
    [SerializeField] private GameEvent onGameStartedEvent;

    private void Start()
    {
        StartCoroutine(LoadHUDAndRaiseEvent());
    }

    private IEnumerator LoadHUDAndRaiseEvent()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameHUD", LoadSceneMode.Additive);
        yield return new WaitUntil(() => operation.isDone);
                
        onGameStartedEvent.Raise();
    }
}