namespace PURR {
	using UnityEngine;
	using UnityEngine.Events;

	public class PhysicsTriggeree : Component {
		public UnityEvent onTouched;

		private void OnTriggerEnter2D(Collider2D _) {
			onTouched.Invoke();
		}
	}
}