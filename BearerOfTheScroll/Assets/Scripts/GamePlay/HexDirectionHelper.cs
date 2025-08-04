using System.Collections.Generic;
using UnityEngine;

public static class HexDirectionHelper
{    
    private static readonly List<Vector3Int> directions = new List<Vector3Int>
    {
        new Vector3Int(1, -1, 0),  
        new Vector3Int(1, 0, -1),  
        new Vector3Int(0, 1, -1),  
        new Vector3Int(-1, 1, 0), 
        new Vector3Int(-1, 0, 1),  
        new Vector3Int(0, -1, 1)   
    };

    
    public static List<Vector3Int> GetAllDirections() => directions;

    
    public static List<Vector3Int> GetDirectionalSpread(Vector3Int forward)
    {
        int index = directions.IndexOf(forward);
        if (index == -1)
        {
            Debug.LogWarning($"direction {forward} not found");
            return GetAllDirections();
        }

        int left = (index + 5) % 6;
        int right = (index + 1) % 6;

        return new List<Vector3Int>
        {
            directions[left],
            forward,
            directions[right]
        };
    }

    public static Vector3Int GetDirection(Vector3 from, Vector3 to)
    {
       
        Vector3Int fromCube = WorldToCube(from);
        Vector3Int toCube = WorldToCube(to);

        Vector3Int delta = toCube - fromCube;
                
        return NormalizeDirection(delta);
    }

    
    public static Vector3Int WorldToCube(Vector3 worldPosition)
    {
        float x = worldPosition.x;
        float z = worldPosition.z;

        float q = (2f / 3f * x);
        float r = (-1f / 3f * x + Mathf.Sqrt(3f) / 3f * z);

        float cubeX = q;
        float cubeZ = r;
        float cubeY = -cubeX - cubeZ;

        return CubeRound(new Vector3(cubeX, cubeY, cubeZ));
    }
        
    public static Vector3Int CubeRound(Vector3 cube)
    {
        int rx = Mathf.RoundToInt(cube.x);
        int ry = Mathf.RoundToInt(cube.y);
        int rz = Mathf.RoundToInt(cube.z);

        float dx = Mathf.Abs(rx - cube.x);
        float dy = Mathf.Abs(ry - cube.y);
        float dz = Mathf.Abs(rz - cube.z);

        if (dx > dy && dx > dz)
        {
            rx = -ry - rz;
        }
        else if (dy > dz)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Vector3Int(rx, ry, rz);
    }

    
    public static Vector3Int NormalizeDirection(Vector3Int delta)
    {
        if (delta == Vector3Int.zero)
            return Vector3Int.zero;

        foreach (var dir in directions)
        {
            if (delta == dir || delta == dir * 2 || delta == dir * 3 || delta == dir * 4)
                return dir;
        }

        Vector3Int bestMatch = directions[0];
        float bestDot = Vector3.Dot(delta, directions[0]);


        foreach (var dir in directions)
        {
            float dot = Vector3.Dot(delta, dir);
            if (dot > bestDot)
            {
                bestDot = dot;
                bestMatch = dir;
            }
        }

        Debug.LogWarning($"Inpossible normalize direction: {delta}");
        return bestMatch; // fallback
    }
}
