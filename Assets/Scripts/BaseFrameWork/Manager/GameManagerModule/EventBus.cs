using UnityEngine;
using System.Collections.Generic;
using System;

//Anonymous Lambda Function is cannot be unsubscribed if use lamda, use a delegate to wrap it.
public class EventBus : IModule
{
    private Dictionary<Type, Action<Event>> eventTable = new Dictionary<Type, Action<Event>>();

    public void Subscribe<T>(Action<T> callback) where T : Event
    {
        if (typeof(T)==typeof(Event))
        {
            Debug.LogError("Cannot subscribe to base Event type directly. Please use a derived event type.");
            return;
        }
        Type eventType = typeof(T);
        if (!eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = delegate { };
        }
        eventTable[eventType] += (e) => callback((T)e);
    }

    public void Unsubscribe<T>(Action<T> callback) where T : Event
    {
        if (typeof(T) == typeof(Event))
        {
            Debug.LogError("Cannot subscribe to base Event type directly. Please use a derived event type.");
            return;
        }
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

    void IModule.AwakeModule()
    {
       //module awake here  
    }
}
