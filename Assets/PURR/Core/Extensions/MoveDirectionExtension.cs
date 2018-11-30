namespace PURR {
	using static UnityEngine.EventSystems.MoveDirection;
	using Unity.Mathematics;
	using UnityEngine.EventSystems;

	///<summary>Adds `int2`, `float2` conversion methods to `MoveDirection`.</summary>
	public static class MoveDirectionExtension {
		public static int2 int2(this MoveDirection direction) {
			switch (direction) {
				case Right: return math.int2(+1,  0);
				case Left:  return math.int2(-1,  0);
				case Up:    return math.int2( 0, +1);
				case Down:  return math.int2( 0, -1);
				default: return math.int2(0, 0);
			}
		}

		public static float2 float2(this MoveDirection direction) => math.float2(direction.int2());

		public static MoveDirection Opposite(this MoveDirection direction) {
			switch (direction) {
				case Left: return Right;
				case Up: return Down;
				case Right: return Left;
				case Down: return Up;
				default: return None;
			}
		}
	}
}