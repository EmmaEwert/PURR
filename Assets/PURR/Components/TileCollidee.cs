namespace PURR {
	using UnityEngine;
	using UnityEngine.Tilemaps;

	///<summary>Flags a tile with the same `Type` as the prefab's `name` as a grid collider.</summary>
	[AddComponentMenu("# PURR/Tiles/TileCollidee")]
	public class TileCollidee : Component {
		public override void OnImportTile(Tile tile) {
			tile.colliderType = Tile.ColliderType.Grid;
		}
	}
}