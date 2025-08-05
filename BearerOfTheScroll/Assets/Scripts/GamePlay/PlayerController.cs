using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementLimiter movementLimiter;
    [SerializeField] private DirectionalMovementLimiter directionalLimiter;


    private void Awake()
    {
        if (movementLimiter == null)
            movementLimiter = GetComponent<MovementLimiter>();

        if (directionalLimiter == null)
            directionalLimiter = GetComponent<DirectionalMovementLimiter>();
    }
    public void MoveTo(Vector3 targetPosition)
    {
        if (!movementLimiter.CanMove())
        {
            Debug.Log("Movement blocked: Wait for next turn.");
            return;
        }

        if (directionalLimiter != null && !directionalLimiter.IsMoveAllowed(transform.position, targetPosition))
        {
            Debug.Log("Movement blocked: Wrong direction.");
            return;
        }

        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        FindObjectOfType<TurnManager>()?.OnPlayerMoved();
    }
}
