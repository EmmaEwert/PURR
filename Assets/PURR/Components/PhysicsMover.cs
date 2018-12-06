namespace PURR {
	using static Unity.Mathematics.math;
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Moves by adding force to an attached rigidbody.</summary>
	[RequireComponent(typeof(Rigidbody2D))]
	public class PhysicsMover : Component {
		public float force = 16;
		public float busyForce;
		public float maxSpeed = 2;

#pragma warning disable CS0108 // hides inherited member
		private Rigidbody2D rigidbody => GetComponentInChildren<Rigidbody2D>();
#pragma warning restore CS0108

		public void Move(MoveDirection direction) {
			var force = Busy ? busyForce : this.force;
			rigidbody.AddForce(direction.float2() * force);
			var velocity = rigidbody.velocity;
			velocity.x = clamp(velocity.x, -maxSpeed, maxSpeed);
			rigidbody.velocity = velocity;
		}
	}
}