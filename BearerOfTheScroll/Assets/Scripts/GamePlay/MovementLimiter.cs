using UnityEngine;
using System.Collections.Generic;

public class MovementLimiter : MonoBehaviour
{
    [SerializeField] private int maxSpeed = 4;

    private int currentSpeed = 1;
    private Vector3Int? lastDirection = null;

    public bool CanMoveTo(Vector3 from, Vector3 to)
    {
        Vector3Int fromCube = HexDirectionHelper.WorldToCube(from);
        Vector3Int toCube = HexDirectionHelper.WorldToCube(to);
        Vector3Int delta = toCube - fromCube;

        Vector3Int direction = HexDirectionHelper.NormalizeDirection(delta);

        if (!GetAllowedDirections().Contains(direction))
        {
            Debug.Log($"Direction {delta} not allowed. Allowed: {string.Join(", ", GetAllowedDirections())}");
            return false;
        }

        int distance = Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y), Mathf.Abs(delta.z));
        if (distance > currentSpeed)
        {
            Debug.Log($"Too far: {distance} > {currentSpeed}");
            return false;
        }

        return true;
    }

    public void RegisterMove(Vector3 from, Vector3 to)
    {
        Vector3Int direction = HexDirectionHelper.GetDirection(from, to);

        if (lastDirection == null || direction == lastDirection)
        {
            currentSpeed = Mathf.Min(currentSpeed + 1, maxSpeed);
        }
        else
        {
            currentSpeed = 1;
        }

        lastDirection = direction;
    }

    
    public List<Vector3Int> GetAllowedDirections()
    {
        if (lastDirection == null)
            return HexDirectionHelper.GetAllDirections();

        return HexDirectionHelper.GetDirectionalSpread(lastDirection.Value);
    }
        
    public void Reset()
    {
        currentSpeed = 1;
        lastDirection = null;
    }

    public int GetCurrentSpeed() => currentSpeed;
}
