namespace PURR {
	using UnityEngine;

	///<summary>Base class that PURR-based importable components must inherit from.</summary>
	[HelpURL("https://github.com/EmmaEwert/PURR/wiki/Components")]
	public abstract class Component : MonoBehaviour {
		///<summary>Callback after tile is instantiated during tileset import.</summary>
		public virtual void OnImportTile(Tile tile) {}

		///<summary>Callback after prefab is instantiated during tilemap import.</summary>
		public virtual void OnImportObject(Object obj) {}

		private bool busy;
		protected bool Busy {
			get => busy;
			set {
				foreach (var component in GetComponentsInChildren<Component>()) {
					component.busy = value;
				}
			}
		}
	}
}