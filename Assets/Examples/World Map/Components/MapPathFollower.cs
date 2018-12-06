using static UnityEngine.EventSystems.MoveDirection;
using PURR;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(GridMover))]
[RequireComponent(typeof(PhysicsOverlapper))]
public class MapPathFollower : PURR.Component {
	public UnityEvent onFollow;
	public UnityEvent onDone;

	private GridMover mover => GetComponent<GridMover>();
	private PhysicsOverlapper overlapper => GetComponent<PhysicsOverlapper>();

	public async void Follow(MoveDirection direction) {
		if (!overlapper.OverlapBox<MapPath>(direction)) { return; }
		onFollow.Invoke();
		using (new SelectedGameObject()) {
			// Step onto the first path tile.
			await mover.Move(direction);

			// Step onto the next path tile, if any.
			for (var next = Left; next <= Down; ++next) {
				if (next != direction.Opposite() && overlapper.OverlapBox<MapPath>(next)) {
					await mover.Move(direction = next);
					next = Left - 1;
				}
			}

			// Step off the last path tile, if no more were found.
			await mover.Move(direction);
		}
		onDone.Invoke();
	}
}

