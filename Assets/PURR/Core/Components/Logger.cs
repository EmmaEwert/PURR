namespace PURR {
	using UnityEngine;

	///<summary>Provides delegatable methods for logging to Unity's console.</summary>
	public class Logger : Component {
		public void Log(string text) => Debug.Log(text);
		public void LogWarning(string text) => Debug.LogWarning(text);
		public void LogError(string text) => Debug.LogError(text);
	}
}