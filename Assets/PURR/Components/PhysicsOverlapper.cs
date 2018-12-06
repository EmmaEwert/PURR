namespace PURR {
	using static Unity.Mathematics.math;
	using System.Linq;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using Unity.Mathematics;

	///<summary>Supplies an `OverlapBox` method for checking collisions, with optional offset.</summary>
	[HelpURL("https://github.com/EmmaEwert/PURR/wiki/Components#" + nameof(PhysicsOverlapper))]
	public class PhysicsOverlapper : Component {
		[HideInInspector, SerializeField] private Transform origin;

		public override void OnImportObject(Object _) {
			origin = new GameObject("Overlapper").transform;
			origin.parent = transform;
		}

		public Collider2D OverlapBox()                        => OverlapBox<Collider2D>();
		public Collider2D OverlapBox(MoveDirection direction) => OverlapBox<Collider2D>(direction);
		public Collider2D OverlapBox(float2 offset) => OverlapBox<Collider2D>(offset);

		public T OverlapBox<T>() where T : UnityEngine.Component {
			return OverlapBox<T>(MoveDirection.None);
		}
		public T OverlapBox<T>(MoveDirection direction) where T : UnityEngine.Component {
			return OverlapBox<T>(direction.float2());
		}
		public T OverlapBox<T>(float2 offset) where T : UnityEngine.Component {
			var point = float3(origin.position).xy + offset;
			var size = float2(0.5f);
			var angle = 0f;
			var layerMask = 1 << gameObject.layer;
			return typeof(T) == typeof(Collider2D)
				? Physics2D.OverlapBox(point, size, angle, layerMask) as T
				: Physics2D.OverlapBoxAll(point, size, angle, layerMask)
				.Select(c => c.GetComponentInParent<T>())
				.Where(c => c != null)
				.FirstOrDefault();
		}
	}
}

