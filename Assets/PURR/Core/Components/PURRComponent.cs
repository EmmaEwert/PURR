namespace PURR {
	using UnityEngine;

	///<summary>Base class that PURR-based importable components must inherit from.</sumamry>
	public abstract class Component : MonoBehaviour {
		///<summary>Callback after tile is instantiated during tileset import.</summary>
		public virtual void OnImportTile(Tile tile) {}

		///<summary>Callback after prefab is instantiated during tilemap import.</summary>
		public virtual void OnImportObject(Object obj) {}

		private bool paused;
		protected bool Paused {
			get => paused;
			set {
				foreach (var component in GetComponentsInChildren<Component>()) {
					component.paused = value;
				}
			}
		}
	}
}