namespace PURR {
	using static Unity.Mathematics.math;
	using UnityEngine;

	///<summary>Moves by adding impulse to an attached rigidbody.</summary>
	[RequireComponent(typeof(Rigidbody2D))]
	public class PhysicsJumper : Component {
		public float force;

#pragma warning disable CS0108 // hides inherited member
		private Rigidbody2D rigidbody => GetComponentInChildren<Rigidbody2D>();
#pragma warning restore CS0108

		public void JumpRelative() {
			if (Busy) { return; }
			rigidbody.AddRelativeForce(float2(0, 1) * force, ForceMode2D.Impulse);
		}
	}
}