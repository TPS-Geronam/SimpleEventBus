using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace SimpleEventBus.Example
{
    [CreateAssetMenu(menuName = "SimpleEventBus/Example/SimpleEventBus", fileName = "MySimpleEventBus.asset")]
    public class MySimpleEventBus : EventBus
    {
        #region basic

        public event Action<string> OnAnonymousEvent;
        public event Action<EventInfo, string> OnEvent;

        public void InvokeAnonymousEvent(string data) => OnAnonymousEvent?.Invoke(data);

        public void InvokeEvent<T>(IEventEmitter<T> invokingFrom, string data)
        {
            var eventInfo = GetEventInfo(invokingFrom, DateTime.UtcNow);
            OnEvent?.Invoke(eventInfo, data);
        }

        public void InvokeEvent(EventEmitterInfo invokingFrom, string data)
        {
            var eventInfo = GetEventInfo(invokingFrom, DateTime.UtcNow);
            OnEvent?.Invoke(eventInfo, data);
        }

        #endregion basic


        #region custom

        public Event<ValueTuple<string, string>>.Anonymous OnAnonymousCustomEvent;
        public Event<ValueTuple<string, string>> OnCustomEvent;

        public void InvokeAnonymousCustomEvent(ValueTuple<string, string> data)
        {
            if (!OnAnonymousCustomEvent.HasListeners())
                throw new Exception("OnAnonymousCustomEvent has no listeners");
            OnAnonymousCustomEvent.Invoke(data);
        }

        public void InvokeCustomEvent(EventEmitterInfo invokingFrom, ValueTuple<string, string> data)
        {
            if (!OnCustomEvent.HasListeners())
                throw new Exception("OnCustomEvent has no listeners");
            var eventInfo = GetEventInfo(invokingFrom, DateTime.UtcNow);
            OnCustomEvent.Invoke(eventInfo, data);
        }

        #endregion custom


        #region sync

        public delegate string Call(string data);
        public Call OnAnonymousSyncEvent;
        public Event<string>.Synchronous<string> OnSyncEvent;
        public Event<string>.Synchronous<ValueTuple<string, int>> OnSyncMultipleReturnEvent;

        public string InvokeAnonymousSyncEvent(string data)
        {
            if (OnAnonymousSyncEvent == null)
                throw new Exception("OnAnonymousSyncEvent has no listeners");
            // notice: only the last registered listener will return its value to the emitter
            return OnAnonymousSyncEvent.Invoke(data);
        }

        public string[] InvokeSyncEvent(EventEmitterInfo invokingFrom, string data)
        {
            if (!OnSyncEvent.HasListeners())
                throw new Exception("OnSyncEvent has no listeners");
            var eventInfo = GetEventInfo(invokingFrom, DateTime.UtcNow);
            return OnSyncEvent.Invoke(eventInfo, data);
        }

        public ValueTuple<string, int>[] InvokeSyncMultipleReturnEvent(EventEmitterInfo invokingFrom, string data)
        {
            if (!OnSyncMultipleReturnEvent.HasListeners())
                throw new Exception("OnSyncMultipleReturnEvent has no listeners");
            var eventInfo = GetEventInfo(invokingFrom, DateTime.UtcNow);
            return OnSyncMultipleReturnEvent.Invoke(eventInfo, data);
        }

        #endregion sync


        #region async

        public Event<string>.Asynchronous<string> OnAsyncEvent;

        public UniTask<string[]> InvokeAsyncEvent(EventEmitterInfo invokingFrom, string data)
        {
            if (!OnAsyncEvent.HasListeners())
                throw new Exception("OnAsyncEvent has no listeners");
            var eventInfo = GetEventInfo(invokingFrom, DateTime.UtcNow);
            return OnAsyncEvent.Invoke(eventInfo, data);
        }

        #endregion async
    }
}
