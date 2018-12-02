namespace PURR.Grammar {
	using static System.IO.Path;
	using Hime.SDK;
	using Hime.SDK.Output;
	using UnityEditor;
	using UnityEditor.Experimental.AssetImporters;
	using UnityEngine;

	///<summary>Hime EBNF grammar importer to generate parsers.</summary>
	[ScriptedImporter(1, "gram", -3000)]
	internal class HimeImporter : ScriptedImporter {
		///<summary>Generate a parser assembly for a Hime EBNF grammar.</summary>
		public override void OnImportAsset(AssetImportContext ctx) {
			var task = new CompilationTask {
				OutputPath = $"Assets{DirectorySeparatorChar}Plugins{DirectorySeparatorChar}Generated",
				Namespace = "PURR.Generated",
				CodeAccess = Modifier.Public,
				Mode = Mode.Source,
			};
			task.AddInputFile(assetPath);
			var report = task.Execute();
			foreach (var error in report.Errors) {
				Debug.LogError(error, ctx.mainObject);
			}
			foreach (var warning in report.Warnings) {
				Debug.LogWarning(warning, ctx.mainObject);
			}
			if (report.Errors.Count != 0) { return; }
			AssetDatabase.ImportAsset(task.OutputPath + DirectorySeparatorChar + task.GrammarName);
		}
	}
}