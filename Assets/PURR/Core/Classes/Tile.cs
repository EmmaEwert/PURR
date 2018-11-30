namespace PURR {
	using RotaryHeart.Lib.SerializableDictionary;

	///<summary>Representation of a single Tiled tile, with properties</summary>
	public class Tile : UnityEngine.Tilemaps.Tile {
		private Properties properties = new Properties();

		public string this[string key] {
			get {
				if (properties.TryGetValue(key, out var value)) {
					return value;
				}
				return null;
			}
			set => properties[key] = value;
		}
	}
}