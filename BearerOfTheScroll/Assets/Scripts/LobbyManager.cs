using UnityEngine;
using UnityEngine.SceneManagement;

public class MainEntry : MonoBehaviour
{
    private void Start()
    {        
        SceneManager.LoadScene("LobbyUI", LoadSceneMode.Additive);
    }
}
