namespace PURR {
	using System;
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Temporary change of selected gameobject. Selection changes back on `Dispose`.</summary>
	public class SelectedGameObject : IDisposable {
		private GameObject oldSelection;
		private GameObject newSelection;

		public SelectedGameObject(GameObject newSelection = null) {
			oldSelection = EventSystem.current.currentSelectedGameObject;
			this.newSelection = newSelection;
			EventSystem.current.SetSelectedGameObject(newSelection);
			newSelection?.SetActive(true);
		}

		public void Dispose() {
			newSelection?.SetActive(false);
			EventSystem.current?.SetSelectedGameObject(oldSelection);
		}
	}
}