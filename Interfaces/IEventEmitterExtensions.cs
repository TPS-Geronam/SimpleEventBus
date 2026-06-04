namespace SimpleEventBus
{
	public static class IEventEmitterExtensions
	{
        public static EventEmitterInfo GetEmitterInfo<T>(this IEventEmitter<T> emitter)
            => emitter.SelfEmitterInfo;
    }
}
