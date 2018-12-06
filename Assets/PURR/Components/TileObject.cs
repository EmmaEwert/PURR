namespace PURR {
	///<summary>Flags a tile type as an object to be instantiated.</summary>
	public class TileObject : Component {
		public override void OnImportTile(Tile tile) {
			tile.gameObject = gameObject;
		}
	}
}