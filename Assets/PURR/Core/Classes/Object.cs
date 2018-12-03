namespace PURR {
	using RotaryHeart.Lib.SerializableDictionary;
	using System;
	using UnityEngine;

	///<summary>Representation of a single Tiled object, with properties</summary>
	[Serializable]
	public class Object {
		public Tile tile;
		[SerializeField] public Properties properties = new Properties();

		public string this[string key] {
			get {
				if (properties.TryGetValue(key, out var objectValue)) {
					return objectValue;
				} else if (tile.properties.TryGetValue(key, out var tileValue)) {
					return tileValue;
				}
				return null;
			}
			set => properties[key] = value;
		}
	}
}