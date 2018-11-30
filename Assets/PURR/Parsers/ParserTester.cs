namespace PURR {
	using global::Hime.Redist;
	using UnityEngine;

	///<summary>Component for testing the Lýsing grammar. Temporary.</summary>
	public class ParserTester : MonoBehaviour {
		[TextArea(1,16)] public string text;
		public ASTNode root;

		///<summary>Parse a text with the Lýsing grammar, and update ingame text to match</summary>
		public void Test() {
			var lýsing = new Lýsing(text);
			root = lýsing.root;
			FindObjectOfType<TMPro.TMP_Text>().text = lýsing.text;
			FindObjectOfType<TMPro.TMP_Text>().ForceMeshUpdate();
			FindObjectOfType<TMPro.TMP_Text>().UpdateMeshPadding();
		}
	}

}