namespace PURR {
	///<summary>Flags a tile type as a grid collider.</summary>
	public class TileCollidee : Component {
		public override void OnImportTile(Tile tile) {
			tile.colliderType = Tile.ColliderType.Grid;
		}
	}
}