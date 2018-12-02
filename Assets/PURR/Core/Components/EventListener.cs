namespace PURR {
	using static UnityAsync.Await;
	using UnityEngine.Events;

	public class EventListener : Component {
		public Event @event;
		public UnityEvent onEvent;

		private void Awake() {
			@event.listeners.Add(onEvent);
		}

		private async void OnDestroy() {
			// Avoid mutating the `event.listeners` collection while it's being iterated, hopefully
			await NextUpdate();
			@event.listeners.Remove(onEvent);
		}
	}
}