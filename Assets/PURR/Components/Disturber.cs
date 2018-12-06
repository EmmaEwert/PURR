namespace PURR {
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<sumamry>Notifies a `Disturbee` to call its delegates when disturbing its child collider.</summary>
	[RequireComponent(typeof(PhysicsOverlapper))]
	public class Disturber : Component {
		private PhysicsOverlapper overlapper => GetComponent<PhysicsOverlapper>();

		public void Disturb() {
			if (Busy) { return; }
			overlapper.OverlapBox<Disturbee>()?.Disturb();
		}
	}
}
