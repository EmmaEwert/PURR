namespace PURR {
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	[CreateAssetMenu]
	public class Event : ScriptableObject {
		public List<UnityEvent> listeners = new List<UnityEvent>();

		public void Invoke() {
			foreach (var listener in listeners) {
				listener.Invoke();
			}
		}
	}
}