using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProcess : MonoBehaviour
{
    [Header("Gameplay")] 
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private MovementHighlighter movementHighlighter;

    [Header("Events")]
    [SerializeField] private GameEvent onGameStartedEvent;
    [SerializeField] private PlayerSpawnEvent onPlayerSpawnedEvent;

    private void Start()
    {
        //Debug.Log("GameProcess is working!");

        Vector3 spawnPosition = new Vector3(1.732051f, 0.4f, -21f);

        GameObject playerGO = Instantiate(playerPrefab, spawnPosition, Quaternion.identity, transform);

        PlayerController player = playerGO.GetComponent<PlayerController>();

        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();

        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(player.transform);
        }

        List<Vector3> availablePositions = FindAvailableHexesNear(spawnPosition); 
        movementHighlighter.ShowAvailableTiles(spawnPosition, availablePositions);

        if (onGameStartedEvent !=null)
        {
            onGameStartedEvent.Raise();
        }

        if (onPlayerSpawnedEvent != null)
        {
            onPlayerSpawnedEvent.Raise(player);
            //Debug.Log("Event PlayerSpawnEvent Goooo");
        }

            
    }

    private List<Vector3> FindAvailableHexesNear(Vector3 center)
    {
        List<Vector3> result = new List<Vector3>();

        foreach (HexTile hex in FindObjectsOfType<HexTile>())
        {
            float distance = Vector3.Distance(center, hex.transform.position);
            if (distance <= 2.0f)
            {
                result.Add(hex.transform.position);
            }
        }

        return result;
    }

    public void HighlightAvailableTiles(Vector3 playerPosition)
    {
        List<Vector3> availablePositions = FindAvailableHexesNear(playerPosition);

        movementHighlighter.ShowAvailableTiles(playerPosition, availablePositions);

        MovementLimiter limiter = FindObjectOfType<MovementLimiter>();
        if (limiter != null)
        {
            limiter.SetAvailableHexes(availablePositions);
        }
    }
}
