namespace PURR {
	using UnityEngine;
	using UnityEngine.EventSystems;

	/// <summary>Sets this `GameObject` as the first selected in the EventSystem, if any.</summary>
	public class Selectee : Component {
		private void Awake() {
			if (!EventSystem.current) { return; }
			EventSystem.current.firstSelectedGameObject = gameObject;
		}
	}
}
