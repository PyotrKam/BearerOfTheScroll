using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private int turnCounter = 1;

    public void EndPlayerTurn()
    {
        Debug.Log($"Turn {turnCounter} ended.");
        turnCounter++;

               
        // - AI turn
        // - Victory chek
        // - Lives check
    }
}