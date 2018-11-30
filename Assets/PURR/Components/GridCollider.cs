namespace PURR {
	using static Unity.Mathematics.math;
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Allows `GridMover` to collide with things.</summary>
	[AddComponentMenu("# PURR/Grid/Actors/Collider")]
	[RequireComponent(typeof(GridMover))]
	[RequireComponent(typeof(GridPhysicsCaster))]
	public class GridCollider : Component {
		private GridPhysicsCaster caster => GetComponent<GridPhysicsCaster>();
		private GridMover mover => GetComponent<GridMover>();

		public async void TryStepAsync(MoveDirection direction) {
			if (caster.BoxCast(direction).Length == 0) {
				await mover.Step(direction);
			}
		}
	}
}