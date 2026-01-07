using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Spawn Points (set in scene)")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Events IN")]
    [SerializeField] private PlayerSpawnEvent onPlayerSpawnedEvent;

    [Header("Events OUT (optional)")]
    [SerializeField] private EnemySpawnEvent onEnemySpawnedEvent;

    private void OnEnable()
    {
        onPlayerSpawnedEvent?.Register(OnPlayerSpawned);
    }

    private void OnDisable()
    {
        onPlayerSpawnedEvent?.Unregister(OnPlayerSpawned);
    }

    private void OnPlayerSpawned(PlayerController player)
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("[EnemySpawner] enemyPrefabs пустой!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[EnemySpawner] spawnPoints пустой!");
            return;
        }

        foreach (var sp in spawnPoints)
        {
            if (sp == null) continue;

            var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            var enemyGO = Instantiate(prefab, sp.position, sp.rotation);

            onEnemySpawnedEvent?.Raise(enemyGO);
        }

        Debug.Log($"[EnemySpawner] Spawned {spawnPoints.Length} enemies");
    }
}
