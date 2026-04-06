using System;
using System.Reflection;

namespace SimpleEventBus
{
    public readonly struct EventSource
    {
        public readonly Type type;
        public readonly TypeInfo typeInfo;
        public readonly string callerFilePath;

        public EventSource(Type type, TypeInfo typeInfo, string callerFilePath)
        {
            this.type = type;
            this.typeInfo = typeInfo;
            this.callerFilePath = callerFilePath;
        }
    }
}
