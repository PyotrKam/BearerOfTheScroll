using System;
using UnityEngine;


[CreateAssetMenu(menuName = "GameEvent")]

public class GameEvent : ScriptableObject
{
    private event Action listeners;

    public void Raise()
    {
        Debug.Log($"[GameEvent] {name} — Raise()");
        listeners?.Invoke();
    } 

    public void Register(Action listener)
    {
        Debug.Log($"[GameEvent] {name} — Register: {listener.Method.Name}");
        listeners += listener;
    }

    public void Unregister(Action listener)
    {
        Debug.Log($"[GameEvent] {name} — Unregister: {listener.Method.Name}");
        listeners -= listener;
    }

}
