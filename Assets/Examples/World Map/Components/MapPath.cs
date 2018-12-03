﻿using static Unity.Mathematics.math;
using static UnityAsync.Await;
using static UnityEngine.EventSystems.MoveDirection;
using PURR;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

[RequireComponent(typeof(GridPhysicsCaster))]
public class MapPath : PURR.Component {
	public float tilesPerSecond = 2;

	private GridPhysicsCaster caster => GetComponent<GridPhysicsCaster>();

	public async void Follow(MoveDirection direction) {
		var selected = EventSystem.current?.currentSelectedGameObject;
		if (!selected) { return; }
		using (new SelectedGameObject()) {
			var subject = selected.GetComponent<GridMover>() ?? selected.AddComponent<GridMover>();
			await Follow(direction, subject);
		}
	}

	private async Task Follow(MoveDirection direction, GridMover subject) {
		// Step onto the path tile.
		await subject.Step(direction);

		// Step onto the next path tile, if any.
		for (var next = Left; next <= Down; ++next) {
			foreach (var hit in caster.BoxCast(next)) {
				var path = hit.collider.GetComponent<MapPath>();
				if (next == direction.Opposite() || !(path?.name != this.name)) { continue; }
				await path.Follow(next, subject);
				return;
			}
		}

		// Step off the last path tile, if no more were found.
		await subject.Step(direction);
	}
}
