using UnityEngine;

public class PathObstructionChecker : MonoBehaviour
{    
    [SerializeField] private float hexStepLength = 1.732051f; 
    [SerializeField] private float sampleRadius = 0.35f; 
        
    [SerializeField] private LayerMask hexLayer;            
    [SerializeField] private bool requireWalkableFlag = true;

    [SerializeField] private bool detectHazardOnPath = true;

    public bool IsPathClear(Vector3 from, Vector3 to, int stepCount, Vector3 alignedDir)
    {
        Vector3 start = new Vector3(from.x, 0, from.z);
        Vector3 dir = new Vector3(alignedDir.x, 0, alignedDir.z).normalized;

        for (int i = 1; i <= stepCount; i++)
        {
            Vector3 sample = start + dir * (hexStepLength * i);
                        
            var hits = Physics.OverlapSphere(sample + Vector3.up * 0.1f, sampleRadius, hexLayer);
            if (hits == null || hits.Length == 0)
                return false;

            if (requireWalkableFlag)
            {
                bool ok = false;
                foreach (var h in hits)
                {
                    var tile = h.GetComponentInParent<HexTile>() ?? h.GetComponent<HexTile>();
                    if (tile == null) continue;
                                        
                    if (!tile.enabled) continue;
                    if (tile is object)
                    {                        
                        var hasField = tile.GetType().GetField("Walkable") != null;
                        ok = !hasField || (bool)tile.GetType().GetField("Walkable").GetValue(tile);
                        if (ok) break;
                    }
                }
                if (!ok) return false;
            }
        }

        return true;
    }

    public bool TryFindFirstHazardOnPath(Vector3 from, int stepCount, Vector3 alignedDir, out int hazardStepIndex)
    {
        hazardStepIndex = -1;

        if (!detectHazardOnPath)
            return false;

        Vector3 start = new Vector3(from.x, 0, from.z);
        Vector3 dir = new Vector3(alignedDir.x, 0, alignedDir.z).normalized;

        for (int i = 1; i <= stepCount; i++)
        {
            Vector3 sample = start + dir * (hexStepLength * i);

            var hits = Physics.OverlapSphere(sample + Vector3.up * 0.1f, sampleRadius, hexLayer);
            if (hits == null || hits.Length == 0)
                continue;

            foreach (var h in hits)
            {
                var tile = h.GetComponentInParent<HexTile>() ?? h.GetComponent<HexTile>();
                if (tile == null) continue;
                if (!tile.enabled) continue;

                
                if (tile.IsObstacle) 
                {
                    hazardStepIndex = i;
                    return true;
                }
            }
        }

        return false;
    }
}
