using System;
using System.Reflection;
using System.Collections.Generic;

namespace SimpleEventBus
{
	public class EventEmitterInfoFactory
	{
        static readonly Dictionary<Type, EventEmitterInfo> _emitters = new();

		public static EventEmitterInfo Create<T>(bool cache = true) 
		{
			var type = typeof(T);

			if (_emitters.TryGetValue(type, out var emitterInfo))
				return emitterInfo;

			emitterInfo = (EventEmitterInfo)type.GetCustomAttribute(typeof(EventEmitterInfo));
			if (cache)
				_emitters.Add(type, emitterInfo);

            return emitterInfo;
        }
	}
}
