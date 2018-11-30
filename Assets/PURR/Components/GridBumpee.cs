namespace PURR {
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Runs delegates when bumped into.</summary>
	[AddComponentMenu("# PURR/Grid/Actors/Bumpee")]
	public class GridBumpee : Component {
		public DirectionEvent onBumped;

		public void Bump(MoveDirection direction) {
			if (Paused) { return; }
			onBumped.Invoke(direction);
		}
	}
}