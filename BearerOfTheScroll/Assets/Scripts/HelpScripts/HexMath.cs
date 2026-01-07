using UnityEngine;

public static class HexMath
{
    public static float XZDistSqr(Vector3 a, Vector3 b)
    {
        a.y = 0; b.y = 0;
        return (a - b).sqrMagnitude;
    }
}