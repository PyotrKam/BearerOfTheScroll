using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLimiter : MonoBehaviour
{
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
