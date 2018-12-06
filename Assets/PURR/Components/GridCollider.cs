namespace PURR {
	using static Unity.Mathematics.math;
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Allows `GridMover` to collide with things.</summary>
	[RequireComponent(typeof(GridMover), typeof(PhysicsOverlapper))]
	public class GridCollider : Component {
		private PhysicsOverlapper overlapper => GetComponent<PhysicsOverlapper>();
		private GridMover mover => GetComponent<GridMover>();

		public async void TryStepAsync(MoveDirection direction) {
			if (overlapper.OverlapBox(direction)) { return; }
			await mover.Move(direction);
		}
	}
}