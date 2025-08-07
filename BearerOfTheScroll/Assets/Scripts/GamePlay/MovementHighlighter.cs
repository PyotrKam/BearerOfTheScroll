using System.Collections.Generic;
using UnityEngine;

public class MovementHighlighter : MonoBehaviour
{
    [SerializeField] private GameObject highlightPrefab;

    private readonly List<GameObject> activeHighlights = new List<GameObject>();

    public void ShowAllowedMoves(PlayerController player)
    {
        ClearMarkers();

        HexTile[] allHexes = FindObjectsOfType<HexTile>();

        foreach (var hex in allHexes)
        {
            Vector3 targetPosition = hex.transform.position;

            
            if (player.CanMoveTo(targetPosition))
            {
                GameObject highlight = Instantiate(
                    highlightPrefab,
                    targetPosition + Vector3.up * 0.1f,
                    Quaternion.identity,
                    transform
                );

                activeHighlights.Add(highlight);
            }
        }
    }

    public void ClearMarkers()
    {
        foreach (var obj in activeHighlights)
        {
            if (obj != null)
                Destroy(obj);
        }

        activeHighlights.Clear();
    }
}
