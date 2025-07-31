using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshCollider))]
public class HexTile : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private PlayerSpawnEvent onPlayerSpawnedEvent;

    private PlayerController player;

    private void OnEnable()
    {
        if (onPlayerSpawnedEvent != null)
            onPlayerSpawnedEvent.Register(HandlePlayerSpawned);
    }

    private void OnDisable()
    {
        if (onPlayerSpawnedEvent != null)
            onPlayerSpawnedEvent.Unregister(HandlePlayerSpawned);
    }

    private void HandlePlayerSpawned(PlayerController newPlayer)
    {
        player = newPlayer;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI()) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    OnClicked();
                }
            }
        }
    }

    private void OnClicked()
    {
        Debug.Log($"Hex {name} was choosed");

        if (player != null)
        {
            player.MoveTo(transform.position);
        }
        else
        {
            Debug.LogWarning("No player, event not worked");
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
