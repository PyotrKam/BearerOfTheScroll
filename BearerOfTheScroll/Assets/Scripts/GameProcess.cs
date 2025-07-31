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
        Debug.Log("GameProcess is working!");

        Vector3 spawnPosition = new Vector3(0, 0.4f, 0);

        GameObject playerGO = Instantiate(playerPrefab, spawnPosition, Quaternion.identity, transform);

        PlayerController player = playerGO.GetComponent<PlayerController>();

        if (onGameStartedEvent !=null)
        {
            onGameStartedEvent.Raise();
        }

        if (onPlayerSpawnedEvent != null)
        {
            onPlayerSpawnedEvent.Raise(player);
            Debug.Log("Event PlayerSpawnEvent Goooo");
        }
            
    }
}
