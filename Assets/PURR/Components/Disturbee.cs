namespace PURR {
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;

	///<summary>Runs delegates when bumped into.</summary>
	[AddComponentMenu("# PURR/Grid/Actors/Disturbee")]
	public class Disturbee : Component {
		public UnityEvent onDisturbed;

		public void Disturb() {
			if (Paused) { return; }
			onDisturbed.Invoke();
		}
	}
}