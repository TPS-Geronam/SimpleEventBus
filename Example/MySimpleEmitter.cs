using System;
using Unity.VisualScripting;
using UnityEngine;

namespace SimpleEventBus.Example
{
    [EventEmitterInfo(emitterType: typeof(MySimpleEmitter), withTypeInfo: false)]
    public class MySimpleEmitter : MonoBehaviour, IEventEmitter
    {
        [SerializeField]
        MySimpleEventBus eventBus;

        EventEmitterInfo emitterInfo;
        public EventEmitterInfo GetEventEmitterInfo() => emitterInfo;

        void Awake()
        {
            // you don't have to cache this, the factory will only ever instantiate one info per type
            emitterInfo = EventEmitterInfoFactory.Create<MySimpleEmitter>();
            emitterInfo.SourceRef = this;
            emitterInfo.CancellationToken = destroyCancellationToken;
        }

        public void EmitAnonymously()
        {
            eventBus.InvokeAnonymousEvent("some simple data");
        }

        public void EmitWithInfo()
        {
            eventBus.InvokeEvent(emitterInfo, "some simple data (cached attribute)");
        }

        public void EmitWithInterface()
        {
            eventBus.InvokeEvent(this, "some simple data (interface)");
        }

        public void EmitAnonymouslyCustom()
        {
            eventBus.InvokeAnonymousCustomEvent(("some", "data"));
        }

        public void EmitCustom()
        {
            eventBus.InvokeCustomEvent(emitterInfo, ("some", "data"));
        }

        public void EmitAnonymouslySync()
        {
            var result = eventBus.InvokeAnonymousSyncEvent("123");
            Debug.Log($"[{DateTime.UtcNow.ToLongTimeString()}] MySimpleEmitter received results: {result}");
        }

        public void EmitSync()
        {
            var results = eventBus.InvokeSyncEvent(emitterInfo, "123");
            Debug.Log($"[{DateTime.UtcNow.ToLongTimeString()}] MySimpleEmitter received results: {results.ToCommaSeparatedString()}");
        }

        public void EmitSyncMultipleReturn()
        {
            var results = eventBus.InvokeSyncMultipleReturnEvent(emitterInfo, "123");
            Debug.Log($"[{DateTime.UtcNow.ToLongTimeString()}] MySimpleEmitter received results: {results.ToCommaSeparatedString()}");
        }

        public void EmitAsync() => AwaitAsyncResponses();

        async void AwaitAsyncResponses()
        {
            var task = eventBus.InvokeAsyncEvent(emitterInfo, "123");
            Debug.Log(".... Simple Emitter doing something before awaiting ....");
            var results = await task;
            Debug.Log($"[{DateTime.UtcNow.ToLongTimeString()}] MySimpleEmitter received results: {results.ToCommaSeparatedString()}");
        }
    }
}
