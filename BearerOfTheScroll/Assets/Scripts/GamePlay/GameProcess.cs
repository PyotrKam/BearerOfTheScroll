using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProcess : MonoBehaviour
{
    [Header("Gameplay")] 
    [SerializeField] private GameObject playerPrefab;

    [Header("Events")]
    [SerializeField] private GameEvent onGameStartedEvent;
    [SerializeField] private PlayerSpawnEvent onPlayerSpawnedEvent;

    private void Start()
    {
        //Debug.Log("GameProcess is working!");

        Vector3 spawnPosition = new Vector3(-40.7032f, 0.4f, -31.5f);

        GameObject playerGO = Instantiate(playerPrefab, spawnPosition, Quaternion.identity, transform);

        PlayerController player = playerGO.GetComponent<PlayerController>();

        if (onGameStartedEvent !=null)
        {
            onGameStartedEvent.Raise();
        }

        if (onPlayerSpawnedEvent != null)
        {
            onPlayerSpawnedEvent.Raise(player);
            //Debug.Log("Event PlayerSpawnEvent Goooo");

            MovementHighlighter highlighter = FindObjectOfType<MovementHighlighter>();
            if (highlighter != null)
            {
                highlighter.SetPlayer(player);
                highlighter.Highlight();
                Debug.Log("Highlight invoked");
            }

            else
            {
                Debug.LogWarning("Not found MovementHighlighter in scene.");
            }
        }

        CameraController camController = Camera.main.GetComponent<CameraController>();
        if (camController != null)
            camController.SetTarget(player.transform);
    }
}
