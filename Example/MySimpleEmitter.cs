using System;
using Unity.VisualScripting;
using UnityEngine;

namespace SimpleEventBus.Example
{
    [EventEmitterInfo(emitterType: typeof(MySimpleEmitter), withTypeInfo: false)]
    public class MySimpleEmitter : MonoBehaviour, IEventEmitter<MySimpleEmitter>
    {
        [SerializeField]
        MySimpleEventBus eventBus;

        EventEmitterInfo EmitterInfo => this.GetEmitterInfo();

        void Awake()
        {
            EmitterInfo.SourceRef = this;
            EmitterInfo.CancellationToken = destroyCancellationToken;
        }

        public void EmitAnonymously()
        {
            eventBus.InvokeAnonymousEvent("some simple data");
        }

        public void EmitWithInfo()
        {
            eventBus.InvokeEvent(EmitterInfo, "some simple data (cached attribute)");
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
            eventBus.InvokeCustomEvent(EmitterInfo, ("some", "data"));
        }

        public void EmitAnonymouslySync()
        {
            var result = eventBus.InvokeAnonymousSyncEvent("123");
            Debug.Log($"[{DateTime.UtcNow.ToLongTimeString()}] MySimpleEmitter received results: {result}");
        }

        public void EmitSync()
        {
            var results = eventBus.InvokeSyncEvent(EmitterInfo, "123");
            Debug.Log($"[{DateTime.UtcNow.ToLongTimeString()}] MySimpleEmitter received results: {results.ToCommaSeparatedString()}");
        }

        public void EmitSyncMultipleReturn()
        {
            var results = eventBus.InvokeSyncMultipleReturnEvent(EmitterInfo, "123");
            Debug.Log($"[{DateTime.UtcNow.ToLongTimeString()}] MySimpleEmitter received results: {results.ToCommaSeparatedString()}");
        }

        public void EmitAsync() => AwaitAsyncResponses();

        async void AwaitAsyncResponses()
        {
            var task = eventBus.InvokeAsyncEvent(EmitterInfo, "123");
            Debug.Log(".... Simple Emitter doing something before awaiting ....");
            var results = await task;
            Debug.Log($"[{DateTime.UtcNow.ToLongTimeString()}] MySimpleEmitter received results: {results.ToCommaSeparatedString()}");
        }
    }
}
