namespace PURR {
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Notifies a `GridBumpee` to call its delegates when bumping into its child collider.</summary>
	[RequireComponent(typeof(PhysicsOverlapper))]
	public class GridBumper : Component {
		private PhysicsOverlapper overlapper => GetComponent<PhysicsOverlapper>();

		public void Bump(MoveDirection direction) {
			if (Paused) { return; }
			overlapper.OverlapBox<GridBumpee>(direction)?.Bump(direction);
		}
	}
}