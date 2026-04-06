using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SimpleEventBus
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EventEmitterInfo : Attribute
    {
        public EventSource Source { get; }

        // note: attributes are destroyed when not in use, so SourceRef and CT will be default
        //       when getting the attribute by ways of Type.GetCustomAttribute(...)
        public object SourceRef { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public EventEmitterInfo(
            Type emitterType,
            bool withTypeInfo = false,
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/caller-information
            [CallerFilePath] string callerFilePath = "")
        {
            var emitterTypeInfo = withTypeInfo ? emitterType.GetTypeInfo() : null;
            Source = new(emitterType, emitterTypeInfo, callerFilePath);
        }
    }
}
