namespace PURR {
	using static UnityAsync.Await;
	using UnityEngine.Events;
	using UnityEngine;

	///<summary>Runs delegates when a specific event is invoked.</summary>
	public class EventListener : MonoBehaviour {
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