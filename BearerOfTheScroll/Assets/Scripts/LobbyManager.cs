using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    private void Start()
    {        
        SceneManager.LoadScene("LobbyUI", LoadSceneMode.Additive);
    }
}
