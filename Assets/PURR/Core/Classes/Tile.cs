namespace PURR {
	using static Unity.Mathematics.math;
	using RotaryHeart.Lib.SerializableDictionary;
	using UnityEngine;
	using UnityEngine.Tilemaps;

	///<summary>Representation of a single Tiled tile, with properties</summary>
	public class Tile : UnityEngine.Tilemaps.Tile {
		public string type;
		[SerializeField] public Properties properties = new Properties();

		public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject gameObject) {
			if (gameObject) {
				gameObject.name = type + " " + location.x + "," + location.y;
				gameObject.transform.position = location; // needs z-axis
				var obj = new Object { tile = this };

				// Call import object callback on each attached component
				foreach (var component in gameObject.GetComponents<Component>()) {
					component.OnImportObject(obj);
				}

				// Align children to center of object
				foreach (Transform child in gameObject.transform) {
					child.localPosition = float3(0.5f, 0.5f, 0);
				}
			}
			return true;
		}

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