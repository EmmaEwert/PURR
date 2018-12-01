namespace PURR {
	using static UnityAsync.Await;
	using UnityEngine.Events;

	public class EventListener : Component {
		public Event @event;
		public UnityEvent onEvent;

		private void OnEnable() {
			@event.listeners.Add(onEvent);
		}

		private async void OnDisable() {
			await NextUpdate();
			@event.listeners.Remove(onEvent);
		}
	}
}