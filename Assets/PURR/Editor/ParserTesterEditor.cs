namespace PURR {
	using global::Hime.Redist;
	using UnityEditor;
	using UnityEngine;

	///<summary>Editor to show abstract syntax trees of Lýsing-parsed texts.</summary>
	[CustomEditor(typeof(ParserTester))]
	internal class ParserTesterEditor : Editor {
		///<summary>Show test button and syntax tree for a text parsed with the Lýsing grammar.</summary>
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			var parser = target as ParserTester;
			if (GUILayout.Button("Test")) {
				parser.Test();
			}
			GUILayout.Label(DisplayTree(parser.root, new bool[0], false));
		}

		///<summary>Traverses to a string the abstract syntax tree of a successfully parsed text.</summary>
		private string DisplayTree(ASTNode node, bool[] crossings, bool last) {
			var prefix = string.Empty;
            for (int i = 0; i < crossings.Length - 1; ++i) {
				prefix += crossings[i] ? "│ " : "  ";
			}
			if (crossings.Length > 0) {
                prefix += last ? "└─" : "├─";
			}
			try {
				prefix += Display(node, crossings);
				for (int i = 0; i < node.Children.Count; ++i) {
					var childCrossings = new bool[crossings.Length + 1];
					System.Array.Copy(crossings, childCrossings, crossings.Length);
					childCrossings[childCrossings.Length - 1] = (i < node.Children.Count - 1);
					prefix += "\n" + DisplayTree(node.Children[i], childCrossings, i == node.Children.Count - 1);
				}
			} catch (System.NullReferenceException) { }
			return prefix;
		}

		///<summary>Display to a string a single node. Helper method.</summary>
		private string Display(ASTNode node, bool[] crossings) {
			var prefix = string.Empty;
            for (int i = 0; i < crossings.Length; ++i) {
				prefix += crossings[i] ? "│ " : "  ";
			}
			return node.SymbolType == SymbolType.Token
				? $"{node.Symbol}\n{prefix}└─\"{node.Value.Replace("\n", @"\n")}\""
				: node.Symbol.ToString();
		}
	}
}
