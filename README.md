# SimpleEventBus

A simple implementation of an event bus system for Unity3D. A bus is a ScriptableObject, can be instantiated as an .asset inside the project hierarchy and then assigned to listeners and emitters. Listeners can be informed about their callers (emitters). Mainly intended for asynchronous events, listeners that return a value and decoupled non-critical game systems.

Last tested on Unity 6.4

<img width="447" height="382" alt="seb" src="https://github.com/user-attachments/assets/14279c8b-ddce-4fc0-8ba5-01059bf0eda3" />

[drawio](https://github.com/TPS-Geronam/SimpleEventBus/blob/main/seb.drawio)

The result datapath doesn't exist for non-{sync, async} events. Emitter-Bus-Listener: n-n-n

## Dependencies
- [UniTask](https://github.com/cysharp/UniTask) for asynchronous events

## Usage
1. implement an event bus and give it some events

```C#
[CreateAssetMenu(menuName = "SimpleEventBus/Example/EventBus", fileName = "MyEventBus.asset")]
public class MyEventBus : EventBus {
	/* C# events */
	public event Action<string> OnAnonymousEvent;
	public event Action<EventInfo, string> OnEvent; // listener knows emitter
	
	/* custom events */
	
	// two input params, no return value
	public Event<ValueTuple<string, string>>.Anonymous OnAnonymousCustomEvent; // listener has no info on emitter
	public Event<ValueTuple<string, string>> OnCustomEvent; // listener knows emitter
	
	// string input, return value, listener knows emitter
	public Event<string>.Synchronous<string> OnSyncEvent; // listener returns string
	public Event<string>.Asynchronous<string> OnAsyncEvent; // listener returns UniTask<string>
	
	// or implement invocation methods for validation
	public void InvokeCustomEvent(EventEmitterInfo invokingFrom, ValueTuple<string, string> params) {
		if (!OnCustomEvent.HasListeners()) throw new Exception("no listeners");
		var eventInfo = GetEventInfo(invokingFrom, DateTime.UtcNow);
		OnCustomEvent.Invoke(eventInfo, params);
	}
}
```

2. listener

```C#
public class MyListener : MonoBehaviour, IEventListener {
	public MyEventBus eventBus;
	
	void OnEnable() => eventBus.OnCustomEvent.Subscribe(HandleCustomEvent);
	void OnDisable() => eventBus.OnCustomEvent.Unsubscribe(HandleCustomEvent);
	
	void HandleCustomEvent(EventInfo eventInfo, ValueTuple<string, string> data) =>
		Debug.Log($"I got u, {((Component)eventInfo.emitter.SourceRef).gameObject.name}");
}
```

3. emitter

```C#
[EventEmitterInfo(emitterType: typeof(MyEmitter), withTypeInfo: false)]
public class MyEmitter : MonoBehaviour, IEventEmitter<MyEmitter> {
	public MyEventBus eventBus;
	// this will create and cache the emitter info
	// cache is inside the EventEmitterInfoFactory
	EventEmitterInfo EmitterInfo => this.GetEmitterInfo();
	
	void Awake() {
		// set the self-ref if you need it
		EmitterInfo.SourceRef = this;
		// the cancel token to be passed to Async events
		EmitterInfo.CancellationToken = destroyCancellationToken;
	}
	
	void work() {
		// invoke an event directly
		var eventInfo = new EventInfo(EmitterInfo, DateTime.UtcNow);
		eventBus.OnCustomEvent.Invoke(eventInfo, ("some", "data"));
		// or implement invocation methods for validation
		eventBus.InvokeCustomEvent(EmitterInfo, ("some", "data"));
		
		// listeners return all results
		var results = eventBus.OnSyncEvent.Invoke(eventInfo, ("123"))
	}
	
	async void workAsync(EventInfo eventInfo) {
		var task = eventBus.OnAsyncEvent.Invoke(eventInfo, "123");
		var results = await task;
	}
}
```

## Thoughts
- events are not optimal for time-critical code
	- not tested in Update, per-frame etc.
- a bus SO should only be instantiated once per system context
	- this keeps members closely grouped instead of spread between bus implementations
	- too generic event buses (e.g. IntEventBus) make the code harder to read and trace
- attributes seem annoying, cooler ways to define class metadata?
- for sync and async events, it's very possible to implement an equivalent of EventInfo so that emitters know what listeners returned which value; alternatively simply also return a `this` reference

## Example Scene
- UI Canvas
	- a bunch of buttons with handlers inside MySimpleEmitter
	- (also possible to directly invoke anonymous events on the bus)
- participants
	- 2 listeners
	- 1 emitter
	- 1 event bus
- Simple
	- anonymous: listener has no info on emitter of event
	- info: emitter passes on its metadata when invoking the bus
	- interface: emitter passes on itself when invoking the bus
- Custom: emitter passes on its metadata when invoking the bus
	- anonymous: listener has no info on emitter of event
- Sync (anonymous)
	- listener returns a value
	- listener has no info on emitter of event
	- only the last listener registered on the event will have its value read by the emitter
- Sync
	- listener returns a value or tuple
	- emitter passes on its metadata when invoking the bus
	- all listener return values will be read by the emitter
- Async
	- listener returns a value
	- emitter passes on its metadata when invoking the bus
	- all listener return values will be awaited and read by the emitter
