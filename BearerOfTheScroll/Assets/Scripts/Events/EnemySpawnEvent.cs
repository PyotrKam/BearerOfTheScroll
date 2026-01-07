using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/EnemySpawnEvent")]
public class EnemySpawnEvent : ScriptableObject
{
    private UnityAction<GameObject> listeners;

    public void Raise(GameObject enemy)
    {
        listeners?.Invoke(enemy);
    }

    public void Register(UnityAction<GameObject> listener)
    {
        listeners += listener;
    }

    public void Unregister(UnityAction<GameObject> listener)
    {
        listeners -= listener;
    }
}
