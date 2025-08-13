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

        Vector3 spawnPosition = new Vector3(1.732051f, 0.4f, -21f);

        GameObject playerGO = Instantiate(playerPrefab, spawnPosition, Quaternion.identity, transform);

        PlayerController player = playerGO.GetComponent<PlayerController>();

        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(player.transform);
        }

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
}
