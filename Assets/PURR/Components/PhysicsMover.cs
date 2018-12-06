namespace PURR {
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Moves by adding force to an attached rigidbody.</summary>
	[RequireComponent(typeof(Rigidbody2D))]
	public class PhysicsMover : Component {
		public float force;

#pragma warning disable CS0108 // hides inherited member
		private Rigidbody2D rigidbody => GetComponentInChildren<Rigidbody2D>();
#pragma warning restore CS0108

		public void Move(MoveDirection direction) {
			rigidbody.AddForce(direction.float2() * force);
		}
	}
}