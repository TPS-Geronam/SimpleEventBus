namespace SimpleEventBus
{
    public interface IEventEmitter<T>
    {
        public EventEmitterInfo SelfEmitterInfo => EventEmitterInfoFactory.Create<T>();
    }
}
