namespace PURR {
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	[CreateAssetMenu]
	public class Event : ScriptableObject {
		public List<UnityEvent> listeners = new List<UnityEvent>();

		public void Invoke() {
			var count = listeners.Count;
			for (var i = 0; i < count; ++i) {
				listeners[i].Invoke();
			}
		}
	}
}