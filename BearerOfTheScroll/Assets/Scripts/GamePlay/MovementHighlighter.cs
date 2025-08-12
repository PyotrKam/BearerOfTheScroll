using UnityEngine;

public class MovementHighlighter : MonoBehaviour
{
    //ZAEBALO
    [SerializeField] private GameObject highlightPrefab;

    public void ShowAllowedMoves(PlayerController player)
    {
        ClearMarkers();

        HexTile[] allHexes = FindObjectsOfType<HexTile>();
        foreach (var hex in allHexes)
        {
            if (hex.transform.IsChildOf(transform))
                continue;

            Vector3 pos = hex.transform.position;

            if (player.CanMoveTo(pos)) 
            {
                Instantiate(highlightPrefab, pos + Vector3.up * 0.1f, Quaternion.identity, transform);
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
}
