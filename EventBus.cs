using System;
using UnityEngine;

namespace SimpleEventBus
{
    public abstract class EventBus : ScriptableObject, IEventBus
    {
        protected EventInfo GetEventInfo<T>(IEventEmitter<T> invokingFrom, in DateTime timestamp)
            => GetEventInfo(invokingFrom.EmitterInfo, timestamp);

        protected EventInfo GetEventInfo(EventEmitterInfo invokingFrom, in DateTime timestamp)
            => new(invokingFrom, timestamp);
    }
}
