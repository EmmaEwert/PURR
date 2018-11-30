namespace PURR {
	using UnityEngine;
	using UnityEngine.EventSystems;

	/// <summary>
	/// Forces the attached gameobject on `Start` to become the selected object in the EventSystem.
	[AddComponentMenu("# PURR/Selectee")]
	public class Selectee : Component {
		private void Start() {
			EventSystem.current.SetSelectedGameObject(gameObject);
		}
	}
}
