using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrivalObstacleFall : MonoBehaviour
{
    [SerializeField] private FallManager fallManager;
    [SerializeField] private float rayUp = 2f;
    [SerializeField] private float rayDown = 5f;

    private void Awake()
    {        
        if (fallManager == null) fallManager = GetComponent<FallManager>();
    }

    public bool CheckAndHandle()
    {
        HexTile tile = GetHexUnder(transform.position);
        Debug.Log($"[ArrivalObstacleFall] Under tile: {(tile ? tile.name : "NULL")}, obstacle={(tile && tile.IsObstacle)}");
        if (tile != null && tile.IsObstacle)
        {
            fallManager?.Fall();
            return true; 
        }
        return false; 
    }

    private HexTile GetHexUnder(Vector3 worldPos)
    {
        if (Physics.Raycast(worldPos + Vector3.up * rayUp, Vector3.down, out var hit, rayDown + rayUp))
            return hit.collider.GetComponent<HexTile>();
        return null;
    }
}
