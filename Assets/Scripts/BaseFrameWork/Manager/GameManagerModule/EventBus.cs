using UnityEngine;
using System.Collections.Generic;
using System;
public class EventBus : IModule
{
    private Dictionary<Type, Action<Event>> eventTable = new Dictionary<Type, Action<Event>>();
    void IModule.InitModule()
    {
        //initialization code here
    }

    public void Subscribe<T>(Action<T> callback) where T : Event
    {
        Type eventType = typeof(T);
        if (!eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = delegate { };
        }
        eventTable[eventType] += (e) => callback((T)e);
    }

    public void Unsubscribe<T>(Action<T> callback) where T : Event
    {
        Type eventType = typeof(T);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] -= (e) => callback((T)e);
        }
    }

    public void Publish<T>(T eventInstance) where T:Event
    {
        Type eventType = typeof(T);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType]?.Invoke(eventInstance);
        }
        else
        {
            Debug.LogWarning($"No subscribers for event type {eventType}");
        }
    }
}
