namespace PURR {
	using static Unity.Mathematics.math;
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Moves by adding impulse to an attached rigidbody.true</summary>
	[AddComponentMenu("# PURR/Physics/Jumper")]
	[RequireComponent(typeof(Rigidbody2D))]
	public class PhysicsJumper : Component {
		public float force;

#pragma warning disable CS0108
		private Rigidbody2D rigidbody => GetComponentInChildren<Rigidbody2D>();
#pragma warning restore CS0108

		public void JumpRelative() {
			rigidbody.AddRelativeForce(float2(0, 1) * force, ForceMode2D.Impulse);
		}
	}
}