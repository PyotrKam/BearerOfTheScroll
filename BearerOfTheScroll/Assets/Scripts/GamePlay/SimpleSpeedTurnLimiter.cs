using System.Collections.Generic;
using UnityEngine;


public class SimpleSpeedTurnLimiter : MonoBehaviour
{
    [Header("6 Directions")]
    [SerializeField] private List<Vector3> directions = new List<Vector3>(6);

    [Range(0f, 1f)]
    [SerializeField] private float tolerance = 0.9f; 

    public int LastDirIndex { get; private set; } = -1; 
    public int LastMoveLen { get; private set; } = 0;  

    private void Awake()
    {
        
        for (int i = 0; i < directions.Count; i++)
        {
            var d = directions[i]; d.y = 0f;
            if (d.sqrMagnitude > 0f) directions[i] = d.normalized;
        }
    }
    
    public bool Allowed(Vector3 fromWorld, Vector3 toWorld)
    {
        if (LastMoveLen <= 0 || LastDirIndex < 0) return true; 

        int idx = NearestDirIndex(fromWorld, toWorld);
        if (idx < 0) return false;

        int maxDev = (LastMoveLen == 1) ? 2 : (LastMoveLen == 2) ? 1 : 0;
        int diff = Mathf.Abs(idx - LastDirIndex);
        int wrapped = Mathf.Min(diff, 6 - diff);
        return wrapped <= maxDev;
    }
        
    public void NoteMove(Vector3 fromWorld, Vector3 toWorld, int stepCount)
    {
        int idx = NearestDirIndex(fromWorld, toWorld);
        if (idx >= 0)
        {
            LastDirIndex = idx;
            LastMoveLen = Mathf.Max(1, stepCount);
        }
    }
        
    public void FilterInPlace(List<Vector3> candidates, Vector3 fromWorld)
    {
        if (LastMoveLen <= 0 || LastDirIndex < 0) return; 
        for (int i = candidates.Count - 1; i >= 0; i--)
            if (!Allowed(fromWorld, candidates[i])) candidates.RemoveAt(i);
    }

    private int NearestDirIndex(Vector3 fromWorld, Vector3 toWorld)
    {
        Vector3 delta = toWorld - fromWorld; delta.y = 0f;
        if (delta.sqrMagnitude < 1e-6f) return -1;

        Vector3 v = delta.normalized;
        float bestDot = -1f; int bestIdx = -1;

        for (int i = 0; i < directions.Count; i++)
        {
            var d = directions[i];
            if (d.sqrMagnitude < 0.9f) continue;
            float dot = Vector3.Dot(v, d);
            if (dot >= tolerance && dot > bestDot) { bestDot = dot; bestIdx = i; }
        }
        return bestIdx; 
    }
}
