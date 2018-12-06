namespace PURR {
	using static Unity.Mathematics.math;
	using static UnityAsync.Await;
	using System.Threading.Tasks;
	using Unity.Mathematics;
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Moves one unit length in one of the four cardinal directions.</summary>
	public class GridMover : Component {
		public float tilesPerSecond = 2;

		private int2 position;
		private float step;

		public async void MoveAsync(MoveDirection direction) => await Move(direction);

		public async Task Move(MoveDirection direction) {
			if (Paused) { return; }
			Paused = true;
			for (; step < 1; await NextUpdate()) {
				if (this == null) { return; }
				transform.localPosition =
					float3(position, transform.localPosition.z) + lerp(0, direction.float3(), step);
				step += tilesPerSecond * Time.deltaTime;
			}
			Paused = false;
			transform.localPosition = float3(position += direction.int2(), transform.localPosition.z);
			step -= 1;
		}

		private void Awake() => position = int3(transform.position).xy;
	}
}

