namespace PURR {
	using UnityEngine;
	using UnityEngine.Events;

	///<summary>Runs delegates when importing.</summary>
	public class Importee : Component {
		public StringEvent onImportName;

		public override void OnImportObject(Object obj) {
			onImportName.Invoke(name);
		}

		private void OnValidate() {
			for (var i = 0; i < onImportName.GetPersistentEventCount(); ++i) {
				onImportName.SetPersistentListenerState(i, UnityEventCallState.EditorAndRuntime);
			}
		}
	}
}
