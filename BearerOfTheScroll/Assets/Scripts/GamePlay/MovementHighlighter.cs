using UnityEngine;

public class MovementHighlighter : MonoBehaviour
{
    //ZAEBALO
    [SerializeField] private GameObject safeHighlightPrefab;
    [SerializeField] private GameObject dangerHighlightPrefab;

    public void ShowAllowedMoves(PlayerController player)
    {
        ClearMarkers();

        HexTile[] allHexes = FindObjectsOfType<HexTile>();

        foreach (var hex in allHexes)
        {
            if (hex.transform.IsChildOf(transform))
                continue;

            if (!hex.Walkable)
                continue;

            Vector3 pos = hex.transform.position;

            if (player.CanMoveTo(pos)) 
            {
                var prefab = hex.IsObstacle ? dangerHighlightPrefab : safeHighlightPrefab;
                if (prefab == null)
                {
                    
                    continue;
                }

                var marker = Instantiate(prefab, pos + Vector3.up * 0.1f, Quaternion.identity, transform);

                SetLayerRecursively(marker, LayerMask.NameToLayer("Ignore Raycast"));
            }
        }
    }

    public void ClearMarkers()
    {
        int before = transform.childCount;
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        
        var leftovers = GameObject.FindGameObjectsWithTag("Highlight");

        foreach (var go in leftovers)
            if (go != null && go.transform.parent != transform)
                Destroy(go);


    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        if (obj == null) return;
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}
