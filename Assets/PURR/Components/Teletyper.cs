namespace PURR {
	using UnityEngine;

	///<summary>Assigns a text to the static `Teletype` object. Also activates the textbox.</summary>
	[AddComponentMenu("# PURR/Text/Teletyper")]
	public class Teletyper : Component {
		[TextArea(1, 16)] public string text;
		[TextArea(1, 16)] public string sourceText;
		[HideInInspector] [SerializeField] private Lýsing lýsing;

		public override void OnImportObject(Object obj) {
			lýsing = new Lýsing(obj["Text"]);
			text = lýsing.text;
			sourceText = lýsing.sourceText;
		}

		public void Type() {
			Teletype.Reveal(lýsing);
		}
	}
}