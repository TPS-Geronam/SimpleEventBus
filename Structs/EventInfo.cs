using System;

namespace SimpleEventBus
{
    public readonly struct EventInfo
    {
        public readonly EventEmitterInfo emitter;
        public readonly DateTime timestamp;

        public EventInfo(EventEmitterInfo emitter, in DateTime timestamp)
        {
            this.emitter = emitter;
            this.timestamp = timestamp;
        }
    }
}
