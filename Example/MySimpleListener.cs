using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

namespace SimpleEventBus.Example
{
    public class MySimpleListener : MonoBehaviour, IEventListener
    {
        [SerializeField]
        MySimpleEventBus eventBus;

        void OnEnable()
        {
            eventBus.OnAnonymousEvent += HandleAnonymousEvent;
            eventBus.OnEvent += HandleEvent;

            eventBus.OnAnonymousCustomEvent.Subscribe(HandleAnonymousEvent);
            eventBus.OnCustomEvent.Subscribe(HandleCustomEvent);


            eventBus.OnAnonymousSyncEvent += HandleAnonymousSyncEvent;

            eventBus.OnSyncEvent.Subscribe(HandleSyncEvent);
            eventBus.OnSyncMultipleReturnEvent.Subscribe(HandleSyncMultipleReturnEvent);

            eventBus.OnAsyncEvent.Subscribe(HandleAsyncEvent);
        }

        void OnDisable()
        {
            eventBus.OnAnonymousEvent -= HandleAnonymousEvent;
            eventBus.OnEvent -= HandleEvent;

            eventBus.OnAnonymousCustomEvent.Unsubscribe(HandleAnonymousEvent);
            eventBus.OnCustomEvent.Unsubscribe(HandleCustomEvent);


            eventBus.OnAnonymousSyncEvent -= HandleAnonymousSyncEvent;
            eventBus.OnSyncEvent.Unsubscribe(HandleSyncEvent);
            eventBus.OnSyncMultipleReturnEvent.Unsubscribe(HandleSyncMultipleReturnEvent);

            eventBus.OnAsyncEvent.Unsubscribe(HandleAsyncEvent);
        }

        void HandleAnonymousEvent(string data) => Debug.Log($"MySimpleListener received: {data}");

        void HandleEvent(EventInfo eventInfo, string data)
        {
            Debug.Log($"MySimpleListener received from {eventInfo.emitter.Source.type}: {data}");
            LogCaller(eventInfo);
        }

        void HandleAnonymousEvent(ValueTuple<string, string> data) => Debug.Log($"MySimpleListener received: {data}");

        void HandleCustomEvent(EventInfo eventInfo, ValueTuple<string, string> data)
        {
            Debug.Log($"MySimpleListener received from {eventInfo.emitter.Source.type}: {data}");
            LogCaller(eventInfo);
        }

        string HandleAnonymousSyncEvent(string data)
        {
            Debug.Log($"MySimpleListener received: {data}");
            return new string(data.Reverse().ToArray());
        }

        string HandleSyncEvent(EventInfo eventInfo, string data)
        {
            Debug.Log($"MySimpleListener received from {eventInfo.emitter.Source.type}: {data}");
            LogCaller(eventInfo);
            return new string(data.Reverse().ToArray());
        }

        ValueTuple<string, int> HandleSyncMultipleReturnEvent(EventInfo eventInfo, string data)
        {
            Debug.Log($"MySimpleListener received from {eventInfo.emitter.Source.type}: {data}");
            LogCaller(eventInfo);
            var result = new string(data.Reverse().ToArray());
            return (result, result.Length);
        }

        async UniTask<string> HandleAsyncEvent(EventInfo eventInfo, string data)
        {
            Debug.Log($"MySimpleListener received from {eventInfo.emitter.Source.type}: {data}");
            LogCaller(eventInfo);

            await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: eventInfo.emitter.CancellationToken);

            var result = new string(data.Reverse().ToArray());
            result += result;
            return result;
        }

        void LogCaller(EventInfo eventInfo)
        {
            Debug.Log($"Called at [{eventInfo.timestamp.ToLongTimeString()}], " +
                $"caller GO is {((Component)eventInfo.emitter.SourceRef).gameObject.name}, " +
                $"physical location is {eventInfo.emitter.Source.callerFilePath}");
        }
    }
}
