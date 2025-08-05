using UnityEngine;
using System.Collections.Generic;

public class DirectionalMovementLimiter : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> allowedDirections = new List<Vector3>
    {
        new Vector3(1f, 0, 0f),                       
    new Vector3(0.5f, 0, Mathf.Sqrt(3) / 2f),     
    new Vector3(-0.5f, 0, Mathf.Sqrt(3) / 2f),    
    new Vector3(-1f, 0, 0f),                      
    new Vector3(-0.5f, 0, -Mathf.Sqrt(3) / 2f),   
    new Vector3(0.5f, 0, -Mathf.Sqrt(3) / 2f)                
    };

    [Range(0f, 1f)]
    [SerializeField] private float directionTolerance = 0.9f;
        
    [SerializeField] private float maxDistance = 2.0f;

    public bool IsMoveAllowed(Vector3 from, Vector3 to)
    {
        Vector3 delta = (to - from);

        if (delta.magnitude > maxDistance)
            return false;

        Vector3 direction = delta.normalized;

        foreach (var dir in allowedDirections)
        {
            float dot = Vector3.Dot(direction, dir.normalized);
            if (dot >= directionTolerance)
                return true;
        }

        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {        
        Gizmos.color = Color.blue;

        foreach (var dir in allowedDirections)
        {
            Gizmos.DrawLine(transform.position, transform.position + dir.normalized * 2f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
#endif
}
