namespace PURR {
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<sumamry>Notifies a `GridBumpee` to call its delegates when bumping into its child collider.</summary>
	[AddComponentMenu("# PURR/Grid/Actors/Bumper")]
	[RequireComponent(typeof(GridPhysicsCaster))]
	public class GridBumper : Component {
		private GridPhysicsCaster caster => GetComponent<GridPhysicsCaster>();

		public void Bump(MoveDirection direction) {
			if (Paused) { return; }
			foreach (var hit in caster.BoxCast(direction)) {
				hit.collider.GetComponentInParent<GridBumpee>()?.Bump(direction);
			}
		}
	}
}