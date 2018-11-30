namespace PURR.Tiled {
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	///<summary>Access to inaccessible `EditorGUIUtility` methods</summary>
	public static class InternalEditorGUIUtility {
		///<summary>Wrapper for Unity internal `EditorGUIUtility.SetIconForObject`.</summary>
		public static void SetIconForObject(GameObject gameObject, Texture texture) {
			typeof(EditorGUIUtility).InvokeMember(
				"SetIconForObject",
				BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic,
				null, null,
				new object[] { gameObject, texture }
			);
		}
	}
}