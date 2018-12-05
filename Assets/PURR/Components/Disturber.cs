namespace PURR {
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<sumamry>Notifies a `GridBumpee` to call its delegates when bumping into its child collider.</summary>
	[AddComponentMenu("# PURR/Grid/Actors/Disturber")]
	[RequireComponent(typeof(PhysicsOverlapper))]
	public class Disturber : Component {
		private PhysicsOverlapper overlapper => GetComponent<PhysicsOverlapper>();

		public void Disturb() {
			if (Paused) { return; }
			foreach (var collider in overlapper.OverlapBox()) {
				collider.GetComponentInParent<Disturbee>()?.Disturb();
			}
		}
	}
}
