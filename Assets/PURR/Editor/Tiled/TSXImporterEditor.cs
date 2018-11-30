namespace PURR.Tiled {
	using UnityEditor;
	using UnityEditor.Experimental.AssetImporters;
	using UnityEngine;

	///<summary>Editor for TSX assets.</summary>
	[CustomEditor(typeof(TSXImporter))]
	internal class TSXImporterEditor : ScriptedImporterEditor {
		///<summary>Show button for reslicing texture, and inform user when this is necessary.</summary>
		public override void OnInspectorGUI() {
			var tsxImporter = target as TSXImporter;
			var tileset = tsxImporter.tileset;
			var fix = false;

			// GUI
			if (tileset?.tilecount != tileset?.tiles.Length) {
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				GUILayout.Label(
					$"This texture contains {tileset.tiles.Length} sprites, {tileset.tilecount} expected.",
					EditorStyles.wordWrappedMiniLabel
				);
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				fix = GUILayout.Button("Fix Now", GUI.skin.button);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			} else {
				fix = (GUILayout.Button("Reslice Texture", GUI.skin.button));
			}
			base.ApplyRevertGUI();

			if (fix) {
				tsxImporter.FixTexture();
			}
		}

	}

}