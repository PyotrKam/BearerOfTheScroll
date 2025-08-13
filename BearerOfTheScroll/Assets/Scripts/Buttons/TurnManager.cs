using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private Button nextTurnButton;

    private MovementLimiter movementLimiter;

    private PlayerController player;
    private MovementHighlighter movementHighlighter;

    private int turnCounter = 1;


    private void Start()
    {
        Debug.Log("TurnManager Start called");

        movementLimiter = FindObjectOfType<MovementLimiter>();
        player = FindObjectOfType<PlayerController>();
        movementHighlighter = FindObjectOfType<MovementHighlighter>();

        if (movementHighlighter == null) Debug.LogWarning("MovementHighlighter not found!");
        if (player == null) Debug.LogWarning("PlayerController not found!");
        if (movementLimiter == null) Debug.LogWarning("MovementLimiter not found!");

        if (movementHighlighter != null && player != null)
            movementHighlighter.ShowAllowedMoves(player);

        StartCoroutine(FindButtonLater());
    }


    public void EndPlayerTurn()
    {
        Debug.Log($"Turn {turnCounter} ended.");
        turnCounter++;

        movementLimiter?.EnableMovement();
        nextTurnButton.gameObject.SetActive(false);

        if (movementHighlighter != null && player != null)
            movementHighlighter.ShowAllowedMoves(player);

        // - AI turn
        // - Victory chek
        // - Lives check
    }

    public void OnPlayerMoved()
    {
        movementLimiter?.DisableMovement();
        nextTurnButton.gameObject.SetActive(true); 
    }

    private IEnumerator FindButtonLater()
    {
        yield return null;

        if (nextTurnButton == null)
        {
            GameObject btnObj = GameObject.Find("NextTurnButton");
            if (btnObj != null)
            {
                nextTurnButton = btnObj.GetComponent<Button>();
            }
            else
            {
                Debug.LogWarning("Button NextTurnButton not found!");
                yield break;
            }
        }



        nextTurnButton.gameObject.SetActive(false);
        //nextTurnButton.onClick.RemoveAllListeners();
        //nextTurnButton.onClick.AddListener(EndPlayerTurn);
        Debug.Log("EndPlayerTurn listener added.");

        Debug.Log($"Listeners on NextTurnButton: {nextTurnButton.onClick.GetPersistentEventCount()}");

    }
}