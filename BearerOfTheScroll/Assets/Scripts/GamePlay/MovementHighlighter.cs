using System.Collections.Generic;
using UnityEngine;

public class MovementHighlighter : MonoBehaviour
{
    [SerializeField] private GameObject highlightPrefab; 

    private readonly List<GameObject> activeHighlights = new List<GameObject>();

    public void ShowAvailableTiles(Vector3 origin, List<Vector3> availablePositions)
    {
        Debug.Log($"[Highlighter] Showing {availablePositions.Count} available positions");

        ClearMarkers();

        foreach (var pos in availablePositions)
        {
            if (pos == origin) continue;

            GameObject highlight = Instantiate(highlightPrefab, pos + Vector3.up * 0.1f, Quaternion.identity);
            activeHighlights.Add(highlight);
        }
    }

    public void ClearMarkers()
    {
        //Debug.Log($"[Highlighter] Clearing {activeHighlights.Count} highlights");

        foreach (GameObject obj in activeHighlights)
        {
            if (obj != null)
                Destroy(obj);
        }
        activeHighlights.Clear();
    }
}