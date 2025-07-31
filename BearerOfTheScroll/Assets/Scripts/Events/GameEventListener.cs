using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private UnityEvent response;

    private void OnEnable()
    {
        if (gameEvent != null)
        {
            Debug.Log($"[GameEventListener] {name} — Registering");
            gameEvent.Register(OnEventRaised);
        }
            
    }

    private void OnDisable()
    {
        if (gameEvent != null)
        {
            Debug.Log($"[GameEventListener] {name} — Unregistering");
            gameEvent.Unregister(OnEventRaised);
        }
            
    }

    private void OnEventRaised()
    {
        Debug.Log($"[GameEventListener] {name} — Event Raised → Invoking Response");
        response?.Invoke();
    }
}
