using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseEntry : MonoBehaviour
{
    
    void Start()
    {
        SceneManager.LoadScene("LobbyManager");
    }

   
}
