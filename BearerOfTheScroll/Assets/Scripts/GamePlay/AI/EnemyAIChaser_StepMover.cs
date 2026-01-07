using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(StepMover))]
public class EnemyAIChaser_StepMover : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform player;
    [SerializeField] private StepMover stepMover;
    [SerializeField] private MoveStepRules stepRules;
    [SerializeField] private DirectionalMovementLimiter dirLimiter;

    [Header("Move params")]
    [SerializeField] private Vector3 visualOffset = Vector3.zero;

    [Tooltip("Если 0 — возьмём из MoveStepRules.HexStepLength")]
    [SerializeField] private float hexStepLengthOverride = 0f;

    [Tooltip("Допуск попадания в центр тайла (в метрах)")]
    [SerializeField] private float tileSnapTolerance = 0.25f;

    private bool _busy;
    private HexTile[] _tiles;

    private float HexStep => (hexStepLengthOverride > 0.0001f) ? hexStepLengthOverride :
        (stepRules != null ? stepRules.HexStepLength : 1f);

    private void Awake()
    {
        if (stepMover == null) stepMover = GetComponent<StepMover>();
        if (stepRules == null) stepRules = GetComponent<MoveStepRules>();
        if (dirLimiter == null) dirLimiter = GetComponent<DirectionalMovementLimiter>();

        _tiles = FindObjectsOfType<HexTile>();

        if (dirLimiter == null)
            Debug.LogWarning("[EnemyAI] DirectionalMovementLimiter не найден на враге. Направления не будут ограничены.");
    }

    public void DoTurn()
    {
        Debug.Log("[EnemyAI] DoTurn called");

        if (_busy) return;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return;

        var myTile = FindNearestTile(transform.position);
        var playerTile = FindNearestTile(player.position);

        if (myTile == null || playerTile == null) return;

        if (myTile == playerTile)
        {
            HitPlayer();
            return;
        }

        var allowedSteps = stepRules != null ? stepRules.GetAllowedNextSteps() : new List<int> { 1 };

        int bestStep = -1;
        Vector3 bestAlignedDir = Vector3.zero;
        float bestScore = float.PositiveInfinity;

        foreach (int stepCount in allowedSteps)
        {
            
            float targetDist = stepCount * HexStep;
            float min = targetDist - tileSnapTolerance;
            float max = targetDist + tileSnapTolerance;

            foreach (var candidate in _tiles)
            {
                if (candidate == null) continue;
                if (!candidate.Walkable) continue;
                if (candidate.IsObstacle) continue;

                float dist = Mathf.Sqrt(HexMath.XZDistSqr(candidate.transform.position, transform.position));
                if (dist < min || dist > max) continue;

                
                Vector3 alignedDir;
                if (dirLimiter != null)
                {
                    if (!dirLimiter.TryGetAlignedDirection(transform.position, candidate.transform.position, out alignedDir))
                        continue;
                }
                else
                {
                    alignedDir = (candidate.transform.position - transform.position);
                    alignedDir.y = 0f;
                    if (alignedDir.sqrMagnitude < 0.0001f) continue;
                    alignedDir.Normalize();
                }

                
                if (!IsPathWalkable(transform.position, alignedDir, stepCount))
                    continue;
                                
                if (!stepMover.IsMoveAllowedFromHere(alignedDir, stepCount, HexStep))
                    continue;

                Vector3 endPos = transform.position + alignedDir * HexStep * stepCount;
                float score = HexMath.XZDistSqr(endPos, player.position);

                if (score < bestScore)
                {
                    bestScore = score;
                    bestStep = stepCount;
                    bestAlignedDir = alignedDir;
                }
            }
        }

        if (bestStep < 0) return;

        _busy = true;
        stepMover.Play(bestAlignedDir, bestStep, HexStep, visualOffset, () =>
        {
            if (stepRules != null)
                stepRules.CommitStep(bestStep);

            _busy = false;
            
            var newMy = FindNearestTile(transform.position);
            var newPlayer = FindNearestTile(player.position);
            if (newMy != null && newPlayer != null && newMy == newPlayer)
                HitPlayer();
        });
    }

    private bool IsPathWalkable(Vector3 from, Vector3 alignedDir, int stepCount)
    {
        for (int i = 1; i <= stepCount; i++)
        {
            Vector3 p = from + alignedDir * HexStep * i;
            var t = FindNearestTile(p);
            if (t == null) return false;

            if (HexMath.XZDistSqr(t.transform.position, p) > tileSnapTolerance * tileSnapTolerance)
                return false;

            if (!t.Walkable) return false;
            if (t.IsObstacle) return false;
        }
        return true;
    }

    private HexTile FindNearestTile(Vector3 pos)
    {
        HexTile best = null;
        float bestD = float.PositiveInfinity;

        foreach (var t in _tiles)
        {
            if (t == null) continue;
            float d = HexMath.XZDistSqr(t.transform.position, pos);
            if (d < bestD)
            {
                bestD = d;
                best = t;
            }
        }
        return best;
    }

    private void HitPlayer()
    {
        var pc = player.GetComponent<PlayerController>();
        if (pc != null)
            pc.OnEnemyCaught();
        else
            FindObjectOfType<FallManager>()?.Fall();
    }
}
