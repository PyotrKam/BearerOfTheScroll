using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyPlayButton : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level_1");
    }
}
