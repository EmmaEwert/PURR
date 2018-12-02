namespace PURR {
	using Hime.Redist;
	using PURR.Generated;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	///<summary>Representation of text parsed with the Lýsing grammar.</summary>
	[Serializable]
	public class Lýsing : LysingParser.Actions {
		public string text = string.Empty;
		public string plainText = string.Empty;
		public string sourceText = string.Empty;
		public List<int> pauses = new List<int>();
		public ASTNode root;
		private bool insideColor;

		///<summary>Parse input, calling `@`-functions defined in the grammar while parsing.</summary>
		public Lýsing(string input) {
			sourceText = input;
			var lexer = new LysingLexer(input);
			var parser = new LysingParser(lexer, this);
			var parsed = parser.Parse();
			root = parsed.Root;
			if (parsed.Errors.Count > 0) {
				parsed.Errors.ToList().ForEach(Debug.LogError);
				return;
			}
			if (insideColor) { text += "</color>"; }
		}

		///<summary>Add plain text when an `@OnText` rule is encountered.</summary>
		public override void OnText(Symbol head, SemanticBody body) {
			text += body[0].Value;
			plainText += body[0].Value;
		}

		///<summary>Start color tag when an `@OnColor` rule is encountered.</summary>
		public override void OnColor(Symbol head, SemanticBody body) {
			if (insideColor) { this.OnColorReset(head, body); }
			text += $"<color=\"{body[0].Value}\">";
			insideColor = true;
		}

		///<summary>End color tag when an `@OnColorReset` rule is encountered.</summary>
		public override void OnColorReset(Symbol head, SemanticBody body) {
			text += $"</color>";
			insideColor = false;
		}

		///<summary>Add pause entry when an `@OnPause` rule is encountered.</summary>
		public override void OnPause(Symbol head, SemanticBody body) {
			pauses.Add(plainText.Length);
		}
	}
}