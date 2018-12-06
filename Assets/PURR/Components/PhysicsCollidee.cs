namespace PURR {
	using UnityEngine;
	using UnityEngine.Events;

	///<summary>Runs delegates when the collider hits another.</summary>
	[RequireComponent(typeof(Collider2D))]
	public class PhysicsCollidee : Component {
		public UnityEvent onCollisionEnter;
		public UnityEvent onCollisionExit;

		private void OnCollisionEnter2D(Collision2D collision) {
			onCollisionEnter.Invoke();
		}

		private void OnCollisionExit2D(Collision2D collision) {
			onCollisionExit.Invoke();
		}
	}
}
