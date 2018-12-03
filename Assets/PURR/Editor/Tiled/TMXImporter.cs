namespace PURR.Tiled {
	using static Unity.Mathematics.math;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.IO.Compression;
	using System.Xml.Linq;
	using UnityEditor;
	using UnityEditor.Experimental.AssetImporters;
	using UnityEngine;
	using UnityEngine.Tilemaps;
	using System.Linq;
	using System.Reflection;

	///<summary>Tiled TMX tilemap importer.</summary>
	[ScriptedImporter(1, "tmx", 2)]
	internal class TMXImporter : ScriptedImporter {
		///<summary>Construct a tilemap from Tiled, adding named prefab instances based on Type.</summary>
		public override void OnImportAsset(AssetImportContext ctx) {
			Benchmark.StartWatch("Tilemap import");
			var document = XDocument.Load(assetPath).Element("map");

			// Attributes
			var orientation = (string)document.Attribute("orientation");
			var width       =    (int)document.Attribute("width");
			var height      =    (int)document.Attribute("height");
			var tilewidth   =    (int)document.Attribute("tilewidth");
			var tileheight  =    (int)document.Attribute("tileheight");
			var infinite    =    (int)document.Attribute("infinite") != 0;
			var tilesetSources = document.Elements("tileset").Attributes("source");
			if (orientation != "orthogonal") {
				throw new NotImplementedException("Orientation: " + orientation);
			} else if (infinite) {
				throw new NotImplementedException("Infinite: " + infinite);
			}

			// Tilesets
			var assetPathPrefix = Path.GetDirectoryName(assetPath) + Path.DirectorySeparatorChar;
			var tileList = new List<PURR.Tile>(new PURR.Tile[1]); // Global Tile IDs start from 1
			foreach (var tilesetSource in tilesetSources) {
				var tilesetPath = assetPathPrefix + tilesetSource.Value;
				var tileset = AssetDatabase.LoadAssetAtPath<Tileset>(tilesetPath);
				tileList.AddRange(tileset.tiles);
				ctx.DependsOnSourceAsset(tilesetPath);
			}
			var tiles = tileList.ToArray();

			// Grid
			var grid = new GameObject(Path.GetFileNameWithoutExtension(assetPath), typeof(Grid));
			grid.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
			ctx.AddObjectToAsset("grid", grid);
			ctx.SetMainObject(grid);
			var collider = grid.AddComponent<CompositeCollider2D>();
			collider.generationType = CompositeCollider2D.GenerationType.Manual;

			// Layers
			var layers = document
				.Elements()
				.Where(e => e.Name == "layer" || e.Name == "objectgroup")
				.ToArray();
			var depth = layers.Length;
			PrefabLoader.cache.Clear();
			foreach (var layer in layers) {
				var name = (string)layer.Attribute("name");
				var offsetx =  ((float?)layer.Attribute("offsetx") ?? 0) / tilewidth;
				var offsety = -((float?)layer.Attribute("offsety") ?? 0) / tilewidth;
				var layerObject = new GameObject(name);
				layerObject.transform.parent = grid.transform;
				layerObject.transform.localPosition = float3(offsetx, offsety, --depth);

				// Tile layer
				if (layer.Name == "layer") {
					var layerWidth = (int)layer.Attribute("width");
					var layerHeight = (int)layer.Attribute("height");
					var layerData = layer.Element("data");
					var gids = ParseGIDs(
						(string)layerData.Attribute("encoding"),
						(string)layerData.Attribute("compression"),
						layerData.Value,
						layerWidth
					);

					// Tilemap
					var layerTiles = new PURR.Tile[gids.Length];
					for (var i = 0; i < layerTiles.Length; ++i) {
						layerTiles[i] = tiles[gids[i] & 0x1ffffff]; // 3 MSB for flipping
					}

					var size = new Vector3Int(layerWidth, layerHeight, 1);
					var bounds = new BoundsInt(
						Vector3Int.zero,
						size
					);
					var tilemap = layerObject.AddComponent<Tilemap>();
					layerObject.AddComponent<TilemapRenderer>();
					layerObject.AddComponent<TilemapCollider2D>().usedByComposite = true;
					tilemap.SetTilesBlock(bounds, layerTiles);

					// Flipped tiles
					for (var i = 0; i < gids.Length; ++i) {
						var diagonal   = (gids[i] >> 29) & 1;
						var vertical   = (gids[i] >> 30) & 1;
						var horizontal = (gids[i] >> 31) & 1;
						var flips = uint4(diagonal, vertical, horizontal, 0);
						if (any(flips)) {
							var position = new Vector3Int(i % width, i / width, 0);
							var transform = Matrix4x4.TRS(
								float3(0),
								Quaternion.AngleAxis(diagonal * 180, float3(1, 1, 0)),
								float3(1) - (diagonal == 1 ? flips.yzw : flips.zyw) * 2
							);
							tilemap.SetTransformMatrix(position, transform);
						}
					}

				// Object layer
				} else {
					var objects = layer.Elements("object");
					foreach (var obj in objects) {
						var @object = new PURR.Object();

						// Attributes
						var objectID     =    (int)obj.Attribute("id");
						var objectName   = (string)obj.Attribute("name");
						var objectType   = (string)obj.Attribute("type");
						var objectGID    =   (int?)obj.Attribute("gid") ?? 0;
						var objectX      =    (int)obj.Attribute("x") / tileheight;
						var objectY      =   -(int)obj.Attribute("y") / tileheight + height;
						var objectWidth  =   (int?)obj.Attribute("width") ?? 0;
						var objectHeight =   (int?)obj.Attribute("height") ?? 0;

						// Elements
						var objectProperties = obj.Element("properties")?.Elements("property");
						if (objectProperties != null) {
							foreach (var property in objectProperties) {
								var propertyName = (string)property.Attribute("name");
								var propertyValue =
									(string)property.Attribute("value")
									?? property.Value;
								@object[propertyName] = propertyValue;
							}
						}

						if (string.IsNullOrEmpty(objectName)) {
							objectName = $"{objectID}";
						}

						var tile = tiles[objectGID];
						@object.tile = tile;
						string icon = null;
						GameObject gameObject; // Doubles as prefab when object has type

						// Default instantiation when object has no set type
						if (string.IsNullOrEmpty(objectType)) {
							gameObject = new GameObject(objectName);
							icon = "sv_label_0";

						// Warn instantiation when object has type but no prefab was found
						} else if (null == (gameObject = PrefabLoader.Load<PURR.Component>(objectType))) {
							gameObject = new GameObject(objectName);
							icon = "sv_label_6";
							Debug.LogWarning(
								"No prefab named \""
								+ objectType
								+ "\" with at least one PURR.Component could be found, defaulting."
							);

						// Prefab instantiation based on object type
						} else {
							var instantiate = true;
							if (!instantiate) { continue; }
							gameObject = Instantiate(gameObject);
							gameObject.name = objectName;
							foreach (var component in gameObject.GetComponents<PURR.Component>()) {
								component.OnImportObject(@object);
							}
						}

						gameObject.transform.parent = layerObject.transform;

						// Object sprite
						var sprite = tile?.sprite;
						if (sprite) {
							gameObject.transform.localPosition = float3(objectX, objectY, 0);
							if (!gameObject.GetComponent<PURR.Component>()) {
								var renderer = new GameObject("renderer").AddComponent<SpriteRenderer>();
								renderer.transform.parent = gameObject.transform;
								renderer.sprite = sprite;
							}
						} else {
							gameObject.transform.localPosition =
								float3(objectX, objectY - (float)objectHeight / tileheight, 0);
						}

						// Icon
						if (icon != null) {
							InternalEditorGUIUtility.SetIconForObject(
								gameObject,
								EditorGUIUtility.IconContent(icon).image
							);
						}

						// Align children to center of object
						foreach (Transform child in gameObject.transform) {
							child.localPosition = float3(
								float2(objectWidth, objectHeight) / tilewidth / 2f,
								child.localPosition.z
							);
						}
					}
				}
			}
			collider.GenerateGeometry();
			Benchmark.StopWatches();
		}

		///<summary>Decode, decompress, and reorder rows of global tile IDs</summary>
		private uint[] ParseGIDs(string encoding, string compression, string data, int width) {
			// Decoding
			byte[] input;
			switch (encoding) {
				case "base64": input = Convert.FromBase64String(data); break;
				default: throw new NotImplementedException("Encoding: " + (encoding ?? "xml"));
			}

			// Decompression
			byte[] output;
			switch (compression) {
				case null:   output = input;             break;
				case "gzip": output = Decompress(input); break;
				default: throw new NotImplementedException("Compression: " + compression);
			}

			// Parse bytes as uint32 gids
			var gids = new uint[output.Length / 4];
			Buffer.BlockCopy(output, 0, gids, 0, output.Length);
			return gids.Reverse(width);
		}

		///<summary>Decompress using gzip.</summary>
		private byte[] Decompress(byte[] input) {
			using (var inStream = new GZipStream(new MemoryStream(input), CompressionMode.Decompress)) {
				var length = BitConverter.ToInt32(input, input.Length - 4);
				var buffer = new byte[length];
				var outStream = new MemoryStream();
				inStream.Read(buffer, 0, length);
				outStream.Write(buffer, 0, length);
				return outStream.ToArray();
			}
		}
	}
}