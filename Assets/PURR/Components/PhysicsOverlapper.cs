namespace PURR {
	using static Unity.Mathematics.math;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using System.Linq;

	///<summary>Supplies a `BoxCast` method for checking neighbor collisions.</summary>
	[AddComponentMenu("# PURR/Grid/Actors/Physics Overlapper")]
	public class PhysicsOverlapper : Component {
		[HideInInspector] [SerializeField] private Transform origin;

		public override void OnImportObject(Object _) {
			origin = new GameObject("Overlapper").transform;
			origin.parent = transform;
		}

		public T OverlapBox<T>(MoveDirection direction = MoveDirection.None) {
			return Physics2D.OverlapBoxAll(
				float3(origin.position).xy + direction.float2(),
				size: float2(0.5f),
				angle: 0f,
				layerMask: 1 << gameObject.layer)
				.Select(c => c.GetComponent<T>())
				.Where(c => c != null)
				.FirstOrDefault();
		}
	}
}

