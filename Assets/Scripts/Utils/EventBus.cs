using System;
using System.Collections.Generic;
using UnityEngine;

namespace VuvyMerge
{
    public static class EventBus
    {
        private static readonly Dictionary<EventType, Delegate> _eventTable = new Dictionary<EventType, Delegate>();

        public static void Subscribe(EventType eventType, Action listener)
        {
            if (_eventTable.TryGetValue(eventType, out var d))
                _eventTable[eventType] = Delegate.Combine(d, listener);
            else
                _eventTable[eventType] = listener;
        }

        public static void Unsubscribe(EventType eventType, Action listener)
        {
            if (_eventTable.TryGetValue(eventType, out var d))
            {
                var removed = Delegate.Remove(d, listener);
                if (removed == null) _eventTable.Remove(eventType);
                else _eventTable[eventType] = removed;
            }
        }

        public static void Trigger(EventType eventType)
        {
            if (_eventTable.TryGetValue(eventType, out var d) && d is Action a) a();
        }

        public static void Subscribe<T>(EventType eventType, Action<T> listener)
        {
            if (_eventTable.TryGetValue(eventType, out var d))
            {
                if (d is Action<T> existing) _eventTable[eventType] = Delegate.Combine(existing, listener);
                else Debug.Log($"Event '{eventType}' already registered with different signature.");
            }
            else
            {
                _eventTable[eventType] = listener;
            }
        }

        public static void Unsubscribe<T>(EventType eventType, Action<T> listener)
        {
            if (_eventTable.TryGetValue(eventType, out var d) && d is Action<T> existing)
            {
                var removed = Delegate.Remove(existing, listener);
                if (removed == null) _eventTable.Remove(eventType);
                else _eventTable[eventType] = removed;
            }
        }

        public static void Trigger<T>(EventType eventType, T arg)
        {
            if (_eventTable.TryGetValue(eventType, out var d) && d is Action<T> a) a(arg);
        }
    }

    public enum EventType
    {
        None,
        OnLevelStartClick,
        OnGenerateClick,
        OnReturnHomeClick,
        OnMerge,
        OnWarning,
    }
}
