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
        if (movementLimiter != null && !movementLimiter.CanMoveTo(transform.position, targetPosition))
        {
            Debug.Log("Move not allowed by MovementLimiter.");
            return;
        }
               
        movementLimiter?.RegisterMove(transform.position, targetPosition);
                
        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
    }
        
    public void ResetMovement()
    {
        movementLimiter?.Reset();
    }

    public bool CanMoveTo(Vector3 from, Vector3 to)
    {
        return movementLimiter != null && movementLimiter.CanMoveTo(from, to);
    }

    public int GetCurrentSpeed()
    {
        return movementLimiter != null ? movementLimiter.GetCurrentSpeed() : 1;
    }
}
