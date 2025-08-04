using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementLimiter movementLimiter;


    private void Awake()
    {
        if (movementLimiter == null)
            movementLimiter = GetComponent<MovementLimiter>();
    }
    public void MoveTo(Vector3 targetPosition)
    {
        if (!movementLimiter.CanMove())
        {
            Debug.Log("Movement blocked: Wait for next turn.");
            return;
        }

        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        FindObjectOfType<TurnManager>()?.OnPlayerMoved();
    }
}
