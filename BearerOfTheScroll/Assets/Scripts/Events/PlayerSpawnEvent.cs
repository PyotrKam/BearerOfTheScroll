using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/PlayerSpawnEvent")]
public class PlayerSpawnEvent : ScriptableObject
{
    private UnityAction<PlayerController> listeners;

    public void Raise(PlayerController player)
    {
        listeners?.Invoke(player);
    }

    public void Register(UnityAction<PlayerController> listener)
    {
        listeners += listener;
    }

    public void Unregister(UnityAction<PlayerController> listener)
    {
        listeners -= listener;
    }
}
