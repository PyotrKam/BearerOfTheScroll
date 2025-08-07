using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLimiter : MonoBehaviour
{

    private List<Vector3> availableHexes = new List<Vector3>();

    public void SetAvailableHexes(List<Vector3> hexes)
    {
        availableHexes = hexes;
    }

    public bool IsHexAvailable(Vector3 pos)
    {
        foreach (var hex in availableHexes)
        {
            if (Vector3.Distance(hex, pos) < 0.2f)
                return true;
        }

        return false;
    }

    private bool canMove = true;

    public bool CanMove() => canMove;
   
    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }
}
