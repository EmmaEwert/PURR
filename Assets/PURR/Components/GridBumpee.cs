namespace PURR {
	using UnityEngine.EventSystems;

	///<summary>Runs delegates when bumped into.</summary>
	public class GridBumpee : Component {
		public DirectionEvent onBumped;

		public void Bump(MoveDirection direction) {
			if (Busy) { return; }
			onBumped.Invoke(direction);
		}
	}
}