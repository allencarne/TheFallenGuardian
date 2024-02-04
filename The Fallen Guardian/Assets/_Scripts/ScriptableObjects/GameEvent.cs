using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameEvents")]
public class GameEvent : ScriptableObject
{
    // The list of listeners that this event will notify if it is rasied
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise()
    {
        for(int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }
}
