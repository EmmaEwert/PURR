namespace PURR {
	using static Unity.Mathematics.math;
	using UnityEngine;

	///<summary>Supplies a `BoxCast` method for checking neighbor collisions.</summary>
	[AddComponentMenu("# PURR/Grid/Actors/Physics Overlapper")]
	public class PhysicsOverlapper : Component {
		[HideInInspector] [SerializeField] private Transform origin;

		public override void OnImportObject(Object _) {
			origin = new GameObject("Overlapper").transform;
			origin.parent = transform;
		}

		public Collider2D[] OverlapBox() {
			return Physics2D.OverlapBoxAll(
				float3(origin.position).xy,
				size: float2(0.5f),
				angle: 0f,
				layerMask: 1 << gameObject.layer);
		}
	}
}

