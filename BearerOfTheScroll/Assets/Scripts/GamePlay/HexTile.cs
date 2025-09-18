using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshCollider))]
public class HexTile : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private PlayerSpawnEvent onPlayerSpawnedEvent;

    [Header("Flags")]
    [SerializeField] public bool Walkable = true;

    [Tooltip("Can go, but will fall")]
    public bool IsObstacle = false;

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
            if (!Walkable) return;

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
        //Debug.Log($"Hex {name} was choosed");

        if (player != null)
        {
            player.MoveTo(transform.position);

            if (TryGetComponent<FinishTile>(out var finish))
                StartCoroutine(WaitArrivalAndOpenFinish(finish));
        }
        else
        {
            //Debug.LogWarning("No player, event not worked");
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private void OnDrawGizmos()
    {
        if (!Walkable) { Gizmos.color = Color.gray; }
        else if (IsObstacle) { Gizmos.color = Color.red; }
        else { Gizmos.color = Color.green; }

        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.02f, 0.25f);
    }

    private System.Collections.IEnumerator WaitArrivalAndOpenFinish(FinishTile finish)
    {
        var limiter = player != null ? player.GetComponent<MovementLimiter>() : null;
                
        float timeout = 3f, t = 0f;
        while (limiter != null && !limiter.CanMove() && t < timeout)
        {
            t += Time.deltaTime;
            yield return null;
        }

        
        if (player == null) yield break;
        var p = player.transform.position;
        var target = transform.position;
        if ((new Vector3(p.x, 0, p.z) - new Vector3(target.x, 0, target.z)).sqrMagnitude > 0.3f * 0.3f)
            yield break;

        
        finish.OnPlayerArrived();
    }
}
