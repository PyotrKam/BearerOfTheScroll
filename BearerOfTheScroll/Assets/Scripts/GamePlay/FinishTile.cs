using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class FinishTile : MonoBehaviour
{
    [Tooltip("Text for final Hex")]
    [SerializeField] private string finishMessage = "Finish! Level done!";
    [SerializeField] private string uiSceneName = "LevelEndUI";

    public void OnPlayerArrived()
    {

        LevelEndUI.FinishMessage = finishMessage;
                
        var ui = SceneManager.GetSceneByName(uiSceneName);
        if (!ui.IsValid() || !ui.isLoaded)
        {
            SceneManager.LoadScene(uiSceneName, LoadSceneMode.Additive);
        }
        
    }
    
}
