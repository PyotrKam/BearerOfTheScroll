using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementLimiter movementLimiter;
    [SerializeField] private DirectionalMovementLimiter directionalLimiter;

    //-------------
    [SerializeField] private MoveStepRules stepRules;
    [SerializeField] private PathObstructionChecker pathChecker;

    private void Awake()
    {
        if (movementLimiter == null)
            movementLimiter = GetComponent<MovementLimiter>();

        if (directionalLimiter == null)
            directionalLimiter = GetComponent<DirectionalMovementLimiter>();
        //-----------
        if (stepRules == null)
            stepRules = GetComponent<MoveStepRules>();

        if (pathChecker == null) 
            pathChecker = GetComponent<PathObstructionChecker>();
    }
    public void MoveTo(Vector3 targetPosition)
    {
        if (!movementLimiter.CanMove())
        {
            Debug.Log("Movement blocked: Wait for next turn.");
            return;
        }
        //------------
        int stepCount = 0;
        if (stepRules != null && !stepRules.IsDistanceAllowed(transform.position, targetPosition, out stepCount))
        {
            Debug.Log("Movement blocked: step length not allowed now.");
            return;
        }

        /*
        if (directionalLimiter != null && !directionalLimiter.IsMoveAllowed(transform.position, targetPosition))
        {
            Debug.Log("Movement blocked: Wrong direction.");
            return;
        }
        */
        Vector3 alignedDir = Vector3.zero;

        if (directionalLimiter != null && !directionalLimiter.TryGetAlignedDirection(transform.position, targetPosition, out alignedDir))
        {
            Debug.Log("Movement blocked: Wrong direction.");
            return;
        }

        if (pathChecker != null && !pathChecker.IsPathClear(transform.position, targetPosition, stepCount, alignedDir))
        {
            Debug.Log("Movement blocked: path is obstructed (gap or unwalkable hex).");
            return;
        }

        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        //---------
        if (stepRules != null)
            stepRules.CommitStep(stepCount);

        FindObjectOfType<TurnManager>()?.OnPlayerMoved();
    }

    public bool CanMoveTo(Vector3 targetPosition)
    {
        if (!movementLimiter.CanMove())
            return false;

        //--------
        int stepCount = 0;
        if (stepRules != null && !stepRules.IsDistanceAllowed(transform.position, targetPosition, out stepCount))
            return false;

        Vector3 alignedDir = Vector3.zero;
        if (directionalLimiter != null &&
            !directionalLimiter.TryGetAlignedDirection(transform.position, targetPosition, out alignedDir))
            return false;

        if (pathChecker != null &&
            !pathChecker.IsPathClear(transform.position, targetPosition, stepCount, alignedDir))
            return false;


        /*
        if (directionalLimiter != null && !directionalLimiter.IsMoveAllowed(transform.position, targetPosition))
            return false;
        */
        return true;
    }
}
