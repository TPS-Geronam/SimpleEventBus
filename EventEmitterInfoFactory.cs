using System;
using System.Reflection;
using System.Collections.Generic;

namespace SimpleEventBus
{
	public class EventEmitterInfoFactory
	{
        static readonly Dictionary<Type, EventEmitterInfo> _emitterCache = new();

		public static EventEmitterInfo Create<T>(bool cache = true) 
		{
			var type = typeof(T);

			if (_emitterCache.TryGetValue(type, out var emitterInfo))
				return emitterInfo;

			emitterInfo = (EventEmitterInfo)type.GetCustomAttribute(typeof(EventEmitterInfo));
			if (cache)
				_emitterCache.Add(type, emitterInfo);

            return emitterInfo;
        }
	}
}
