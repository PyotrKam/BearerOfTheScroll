using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private Button nextTurnButton;

    private int turnCounter = 1;


    private void Start()
    {
        nextTurnButton.gameObject.SetActive(false); 
    }
    public void EndPlayerTurn()
    {
        Debug.Log($"Turn {turnCounter} ended.");
        turnCounter++;

        nextTurnButton.gameObject.SetActive(false);
        // - AI turn
        // - Victory chek
        // - Lives check
    }

    public void OnPlayerMoved()
    {
        nextTurnButton.gameObject.SetActive(true); 
    }
}