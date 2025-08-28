using UnityEngine;
using System.Collections.Generic;


public class MoveStepRules : MonoBehaviour
{
    [Header("Size of hex steps")]
    [SerializeField] private float hexStepLength = 1f;
        
    [SerializeField] private float distanceTolerance = 0.1f;   
    [SerializeField] private bool firstMoveIsOne = true;
        
    [SerializeField] private int defaultPreviousStep = 1;

    private int? previousStep;
    public float HexStepLength => hexStepLength;


    public bool IsStepCountAllowedNow(int stepCount)
    {
        foreach (var allowed in GetAllowedNextSteps())
            if (allowed == stepCount)
                return true;

        return false;
    }

    
    public bool IsDistanceAllowed(Vector3 from, Vector3 to, out int resolvedStepCount)
    {
        float dist = Vector3.Distance(new Vector3(from.x, 0, from.z), new Vector3(to.x, 0, to.z));
                
        int stepCount = Mathf.RoundToInt(dist / Mathf.Max(0.0001f, hexStepLength));
                
        float targetDist = stepCount * hexStepLength;
        bool nearGrid = Mathf.Abs(dist - targetDist) <= distanceTolerance;

        resolvedStepCount = stepCount;

        if (!nearGrid) return false;

        return IsStepCountAllowedNow(stepCount);
    }
    
    public void CommitStep(int stepCount)
    {
        previousStep = Mathf.Max(1, stepCount);
    }
        
    public void ResetHistory(int? startPrev = null)
    {
        previousStep = startPrev;
    }
        
    public List<int> GetAllowedNextSteps()
    {
        int prev = previousStep ?? (firstMoveIsOne ? 1 : defaultPreviousStep);

        // Rule:
        // 1 -> {1,2}
        // 2 -> {1,2,3}
        // 3 -> {2,3,4}
        // 4+ -> {3,4}
        if (prev <= 1) return new List<int> { 1, 2 };
        if (prev == 2) return new List<int> { 1, 2, 3 };
        if (prev == 3) return new List<int> { 2, 3, 4 };
        return new List<int> { 3, 4 };
    }
}
