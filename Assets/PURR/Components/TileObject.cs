namespace PURR {
	using UnityEngine;

	///<summary>Flags tile as an object.</summary>
	[AddComponentMenu("# PURR/Tile Object")]
	public class TileObject : Component {
		public override void OnImportTile(Tile tile) {
			tile.gameObject = gameObject;
		}
	}
}