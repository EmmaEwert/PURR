namespace PURR {
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Runs delegates while receiving directional input.</summary>
	[AddComponentMenu("# PURR/Digital Move Handler")]
	public class DigitalMoveHandler : Component, IMoveHandler {
		public DirectionEvent onMove;

		public void OnMove(AxisEventData data) {
			if (data.moveDir != MoveDirection.None) {
				onMove.Invoke(data.moveDir);
			}
		}
	}
}
