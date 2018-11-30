namespace PURR {
	using RotaryHeart.Lib.SerializableDictionary;
	using System;

	///<summary>Representation of a single Tiled object, with properties</summary>
	[Serializable]
	public class Object {
		public Tile tile;

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