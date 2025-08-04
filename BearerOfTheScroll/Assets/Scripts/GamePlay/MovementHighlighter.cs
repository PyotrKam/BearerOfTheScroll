using UnityEngine;

public class MovementHighlighter : MonoBehaviour
{
    [SerializeField] private float range = 2.1f;
    private PlayerController player;

    public void SetPlayer(PlayerController newPlayer)
    {
        player = newPlayer;
    }

    public void Highlight()
    {
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned in Highlighter.");
            return;
        }

        HexTile[] tiles = FindObjectsOfType<HexTile>();
        foreach (var tile in tiles)
        {
            var controller = tile.GetComponent<HexAvailabilityController>();

            if (controller != null)
            {
                Vector3 tilePos = tile.transform.position;

                float distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z),
                                new Vector2(tilePos.x, tilePos.z));

                bool canMove = player.CanMoveTo(player.transform.position, tilePos);

                Debug.Log($"Tile: {tile.name}, Distance: {distance:F2}, CanMove: {canMove}");

                controller.SetAvailable(distance <= range && canMove);
                Debug.Log($"Tile: {tile.name}, Distance: {distance:F2}, Direction: {HexDirectionHelper.GetDirection(player.transform.position, tilePos)}, CanMove: {canMove}");
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.transform.position, range);
        }
    }
}
