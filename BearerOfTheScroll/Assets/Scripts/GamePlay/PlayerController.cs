using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public void MoveTo(Vector3 targetPosition)
    {
        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        FindObjectOfType<TurnManager>()?.OnPlayerMoved();
    }
}
