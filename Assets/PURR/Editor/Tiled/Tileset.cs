namespace PURR.Tiled {
	using Unity.Mathematics;
	using UnityEngine;

	///<summary>Representation of a Tiled tileset.</summary>
	public class Tileset : ScriptableObject {
		public string version;
		public string tiledversion;
		public int tilewidth;
		public int tileheight;
		public int spacing;
		public int margin;
		public int tilecount;
		public int columns;

		public int2 tileoffset;
		public string imageSource;

		public Texture2D texture;
		public Tile[] tiles;
	}
}