using Cysharp.Threading.Tasks;
using System;

namespace SimpleEventBus
{
    public struct Event<T>
    {
        event Action<EventInfo, T> OnEvent;

        public void Subscribe(Action<EventInfo, T> action) => OnEvent += action;
        public void Unsubscribe(Action<EventInfo, T> action) => OnEvent -= action;
        public readonly bool HasListeners() => OnEvent != null;
        public readonly void Invoke(EventInfo eventInfo, T data) => OnEvent.Invoke(eventInfo, data);

        public struct Anonymous
        {
            event Action<T> OnEvent;
            
            public void Subscribe(Action<T> action) => OnEvent += action;
            public void Unsubscribe(Action<T> action) => OnEvent -= action;
            public readonly void Invoke(T data) => OnEvent.Invoke(data);
            public readonly bool HasListeners() => OnEvent != null;
        }

        public struct Synchronous<U>
        {
            Func<EventInfo, T, U> OnEvent;

            public void Subscribe(Func<EventInfo, T, U> action) => OnEvent += action;
            public void Unsubscribe(Func<EventInfo, T, U> action) => OnEvent -= action;
            public readonly bool HasListeners() => OnEvent != null;
            public readonly U[] Invoke(EventInfo eventInfo, T data)
            {
                var invocationList = OnEvent.GetInvocationList();
                var results = new U[invocationList.Length];
                for (var i = 0; i < invocationList.Length; i++)
                    results[i] = (U)invocationList[i].DynamicInvoke(eventInfo, data);
                return results;
            }
        }

        public struct Asynchronous<U>
        {
            Func<EventInfo, T, UniTask<U>> OnEvent;

            public void Subscribe(Func<EventInfo, T, UniTask<U>> action) => OnEvent += action;
            public void Unsubscribe(Func<EventInfo, T, UniTask<U>> action) => OnEvent -= action;
            public readonly bool HasListeners() => OnEvent != null;
            public readonly UniTask<U[]> Invoke(EventInfo eventInfo, T data)
            {
                var invocationList = OnEvent.GetInvocationList();
                var taskHandles = new UniTask<U>[invocationList.Length];
                for (var i = 0; i < taskHandles.Length; i++)
                    taskHandles[i] = ((Func<EventInfo, T, UniTask<U>>)invocationList[i]).Invoke(eventInfo, data);
                return UniTask.WhenAll(taskHandles);
            }
        }
    }
}
