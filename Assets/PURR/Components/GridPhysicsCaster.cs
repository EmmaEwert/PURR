namespace PURR {
	using static Unity.Mathematics.math;
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Supplies a `BoxCast` method for checking neighbor collisions.</summary>
	[AddComponentMenu("# PURR/Grid/Actors/Physics Caster")]
	public class GridPhysicsCaster : Component {
		[HideInInspector] [SerializeField] private Transform origin;

		public override void OnImportObject(Object _) {
			origin = new GameObject("Caster").transform;
			origin.parent = transform;
		}

		public RaycastHit2D[] BoxCast(MoveDirection direction) {
			return Physics2D.BoxCastAll(
				float3(origin.position).xy,
				size: float2(0.5f),
				angle: 0,
				direction.float2(),
				distance: 1,
				layerMask: 1 << gameObject.layer
			);
		}
	}
}
