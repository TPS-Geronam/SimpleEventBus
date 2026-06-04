namespace SimpleEventBus
{
    public interface IEventEmitter<T>
    {
        public EventEmitterInfo EmitterInfo => EventEmitterInfoFactory.Create<T>();
    }
}
