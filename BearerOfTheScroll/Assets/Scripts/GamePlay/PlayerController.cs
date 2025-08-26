using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementLimiter movementLimiter;
    [SerializeField] private DirectionalMovementLimiter directionalLimiter;

    [SerializeField] private MoveStepRules stepRules;
    [SerializeField] private PathObstructionChecker pathChecker;

    [Header("Rotation")]
    [SerializeField] private Transform rotateTarget;
    [SerializeField] private bool smoothRotate = true;
    [SerializeField] private float rotateSpeed = 540f;
    [SerializeField] private float yForwardOffsetDeg = 0f;

    private Coroutine _rotateCo;

    private void Awake()
    {
        if (movementLimiter == null)
            movementLimiter = GetComponent<MovementLimiter>();

        if (directionalLimiter == null)
            directionalLimiter = GetComponent<DirectionalMovementLimiter>();
        
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
        
        int stepCount = 0;

        if (stepRules != null && !stepRules.IsDistanceAllowed(transform.position, targetPosition, out stepCount))
        {
            Debug.Log("Movement blocked: step length not allowed now.");
            return;
        }
                
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

        FaceTowards(alignedDir);

        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
       
        if (stepRules != null)
            stepRules.CommitStep(stepCount);

        FindObjectOfType<TurnManager>()?.OnPlayerMoved();
    }

    public bool CanMoveTo(Vector3 targetPosition)
    {
        if (!movementLimiter.CanMove())
            return false;
                
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

        return true;
    }

    private void FaceTowards(Vector3 alignedDir)
    {
        Vector3 fwd = new Vector3(alignedDir.x, 0f, alignedDir.z);
        if (fwd.sqrMagnitude < 1e-6f) return;

        Quaternion targetRot = Quaternion.LookRotation(fwd, Vector3.up);
        if (Mathf.Abs(yForwardOffsetDeg) > 0.01f)
            targetRot = Quaternion.AngleAxis(yForwardOffsetDeg, Vector3.up) * targetRot;

        Transform t = rotateTarget != null ? rotateTarget : transform;

        if (!smoothRotate)
        {
            t.rotation = targetRot;
            return;
        }

        if (_rotateCo != null) StopCoroutine(_rotateCo);
        _rotateCo = StartCoroutine(RotateTo(t, targetRot));
    }

    private System.Collections.IEnumerator RotateTo(Transform t, Quaternion targetRot)
    {
        while (Quaternion.Angle(t.rotation, targetRot) > 0.1f)
        {
            t.rotation = Quaternion.RotateTowards(t.rotation, targetRot, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        t.rotation = targetRot;
    }
}
