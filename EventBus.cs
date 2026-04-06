using System;
using UnityEngine;

namespace SimpleEventBus
{
    public abstract class EventBus : ScriptableObject, IEventBus
    {
        protected EventInfo GetEventInfo(IEventEmitter invokingFrom, in DateTime timestamp)
            => GetEventInfo(invokingFrom.GetEventEmitterInfo(), timestamp);

        protected EventInfo GetEventInfo(EventEmitterInfo invokingFrom, in DateTime timestamp)
            => new(invokingFrom, timestamp);
    }
}
