namespace PURR.Tiled {
	using static Unity.Mathematics.math;
	using System.IO;
	using System.Linq;
	using System.Xml.Linq;
	using Unity.Burst;
	using Unity.Collections;
	using Unity.Jobs;
	using Unity.Mathematics;
	using UnityEditor;
	using UnityEditor.Experimental.AssetImporters;
	using UnityEngine;

	///<summary>Tiled TSX external tileset importer.</summary>
	[ScriptedImporter(2, "tsx", 1)]
	internal class TSXImporter : ScriptedImporter {
		public Tileset tileset;

		///<summary>Construct a tileset from Tiled by instantiating one tile per texture sprite.</summary>
		public override void OnImportAsset(AssetImportContext ctx) {
			Benchmark.StartWatch("Tileset import");
			var document = XDocument.Load(assetPath).Element("tileset");

			// Tileset object
			tileset = ScriptableObject.CreateInstance<Tileset>();
			ctx.AddObjectToAsset("tileset", tileset);

			// Attributes
			tileset.name        = (string )document.Attribute("name");
			tileset.tilewidth   = (int    )document.Attribute("tilewidth");
			tileset.tileheight  = (int    )document.Attribute("tileheight");
			tileset.spacing     = (int?   )document.Attribute("spacing") ?? 0;
			tileset.margin      = (int?   )document.Attribute("margin") ?? 0;
			tileset.tilecount   = (int    )document.Attribute("tilecount");
			tileset.columns     = (int    )document.Attribute("columns");

			// Elements
			var tileoffset  = document.Element("tileoffset");
			tileset.tileoffset  = int2(
				(int?)tileoffset?.Attribute("x") ?? 0,
				(int?)tileoffset?.Attribute("y") ?? 0
			);
			tileset.imageSource = (string)document.Element("image").Attribute("source");
			var complexTiles = document
				.Elements("tile")
				.Select(tile => (
					id:   (int   )tile.Attribute("id"),
					type: (string)tile.Attribute("type")
				)).ToArray();

			// Texture
			Benchmark.StartWatch("Texture");
			var imageAssetPath =
				Path.GetDirectoryName(ctx.assetPath)
				+ Path.DirectorySeparatorChar
				+ Path.GetFileName(tileset.imageSource);
			tileset.texture = AssetDatabase.LoadMainAssetAtPath(imageAssetPath) as Texture2D;
			if (!tileset.texture) {
				Debug.LogError(
					$"Error: could not load texture \"{imageAssetPath}\" of tileset \"{ctx.assetPath}\".",
					ctx.mainObject
				);
				return;
			}
			Benchmark.StopWatch("Texture");

			// Sprites
			Benchmark.StartWatch("Sprites");
			var sprites = AssetDatabase.LoadAllAssetsAtPath(imageAssetPath).OfType<Sprite>().ToArray();
			if (sprites.Length != tileset.tilecount) {
				Debug.LogError(
					$"Error: texture \"{imageAssetPath}\" contains {sprites.Length} sprites, {tileset.tilecount} expected.",
					ctx.mainObject
				);
				return;
			}
			Benchmark.StopWatch("Sprites");

			// Tiles (performance bottleneck)
			Benchmark.StartWatch("Tiles");
			tileset.tiles = new Tile[tileset.tilecount];
			var tilePrefab = ScriptableObject.CreateInstance<Tile>();
			tilePrefab.colliderType = Tile.ColliderType.None;
			for (var i = 0; i < tileset.tilecount; ++i) {
				tileset.tiles[i] = Instantiate(tilePrefab);
				tileset.tiles[i].hideFlags = HideFlags.HideInHierarchy;
				tileset.tiles[i].sprite = sprites[i];
				tileset.tiles[i].name = i.ToString();
				ctx.AddObjectToAsset(tileset.tiles[i].name, tileset.tiles[i]);
			}
			DestroyImmediate(tilePrefab);
			Benchmark.StopWatch("Tiles");

			// Complex tiles, usually typed, animated, or both
			Benchmark.StartWatch("Complex tiles");
			for (var i = 0; i < complexTiles.Length; ++i) {
				var tileid = complexTiles[i].id;
				var tiletype = complexTiles[i].type;
				if (string.IsNullOrEmpty(tiletype)) { continue; }
				var prefab = PrefabLoader.Load<PURR.Component>(tiletype);
				if (!prefab) {
					Debug.LogWarning(
						"No prefab named \""
						+ tiletype
						+ "\" with at least one PURR.Component could be found, skipping."
					);
					continue;
				}
				foreach (var component in prefab.GetComponents<PURR.Component>()) {
					component.OnImportTile(tileset.tiles[tileid]);
				}
			}
			Benchmark.StopWatch("Complex tiles");

			Benchmark.StopWatches();
			// TODO: Reslice texture if already sliced and relevant settings change
		}

		///<summary>Apply tileset settings to texture, and slice into separate sprites.</summary>
		internal void FixTexture() {
			using (var output = new NativeArray<Rect>(tileset.tilecount, Allocator.TempJob)) {
				Benchmark.StartWatch("Fix Texture");
				var jobHandle = new ComputeSpriteRectsJob {
					Output = output,
					columns = tileset.columns,
					rows = tileset.tilecount / tileset.columns,
					spacing =
						int2(tileset.tilewidth, tileset.tileheight).xyxy
						+ int2(tileset.spacing, 0).xxyy,
					margin = int4(tileset.margin, tileset.margin, 0, 0)
				}.Schedule(output.Length, 64);

				Benchmark.StartWatch("Texture settings");
				// Texture settings
				var imageAssetPath =
					Path.GetDirectoryName(assetPath)
					+ Path.DirectorySeparatorChar
					+ Path.GetFileName(tileset.imageSource);
				var textureImporter = AssetImporter.GetAtPath(imageAssetPath) as TextureImporter;
				textureImporter.textureType = TextureImporterType.Sprite;
				// TODO: must convert to single mode and back, if it's already multiple?
				textureImporter.spriteImportMode = SpriteImportMode.Multiple;
				textureImporter.spriteBorder = float4(0f);
				textureImporter.sRGBTexture = true;
				textureImporter.alphaSource = TextureImporterAlphaSource.FromInput;
				textureImporter.alphaIsTransparency = true;
				textureImporter.isReadable = false;
				textureImporter.mipmapEnabled = false;
				textureImporter.wrapMode = TextureWrapMode.Clamp;
				textureImporter.filterMode = FilterMode.Point;
				var settings = new TextureImporterSettings();
				textureImporter.ReadTextureSettings(settings);
				settings.spriteMeshType = SpriteMeshType.FullRect;
				settings.spriteExtrude = 0;
				settings.spriteGenerateFallbackPhysicsShape = false;
				textureImporter.SetTextureSettings(settings);
				Benchmark.StopWatch("Texture settings");

				Benchmark.StartWatch("Wait for job");
				jobHandle.Complete();
				Benchmark.StopWatch("Wait for job");

				// Sprite slicing
				Benchmark.StartWatch("Sprite slicing");
				var spritesheet = new SpriteMetaData[output.Length];
				for (var i = 0; i < output.Length; ++i) {
					spritesheet[i].alignment = (int)SpriteAlignment.Custom;
					spritesheet[i].border = float4(0);
					spritesheet[i].name = Path.GetFileNameWithoutExtension(tileset.imageSource) + i.ToString();
					spritesheet[i].pivot =
						float2(0.5f)
						- tileset.tileoffset
						/ float2(tileset.tilewidth, tileset.tileheight);
					spritesheet[i].rect = output[i];
				}
				textureImporter.spritesheet = spritesheet;
				Benchmark.StopWatch("Sprite slicing");

				Benchmark.StartWatch("Reimport image");
				AssetDatabase.ImportAsset(textureImporter.assetPath, ImportAssetOptions.ForceUncompressedImport);
				Benchmark.StopWatch("Reimport image");
				Benchmark.StopWatch("Fix Texture");
				Benchmark.StopWatches();
				AssetDatabase.ImportAsset(assetPath);
			}
		}

		///<summary>Populate a Rect array with tile positions and dimensions.</summary>
		[BurstCompile(Accuracy = Accuracy.Low, Support = Support.Relaxed)]
		private struct ComputeSpriteRectsJob : IJobParallelFor {
			[WriteOnly] public NativeArray<Rect> Output;
			[ReadOnly] public int columns;
			[ReadOnly] public int rows;
			[ReadOnly] public int4 spacing;
			[ReadOnly] public int4 margin;

			public void Execute(int index) {
				var rect = mad(spacing, int4(
					(index % columns),
					(rows - index / columns - 1),
					1,
					1
				), margin);
				Output[index] = new Rect(float2(rect.xy), float2(rect.zw));
			}
		}
	}
}