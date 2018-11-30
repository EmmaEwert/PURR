namespace PURR.Tiled {
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;

	///<summary>Helper methods for loading prefabs based on name and type.</summary>
	internal static class PrefabLoader {
        public static Dictionary<string, GameObject> cache = new Dictionary<string, GameObject>();

		///<summary>Load first prefab with `name` and one or more components of given type.</summary>
        public static GameObject Load<T>(string name) where T : Behaviour {
            if (name == null || name == "") { return null; }
            if (cache.TryGetValue(name, out var cachedPrefab)) {
                return cachedPrefab;
            }
            var filename = name + ".prefab";
            var asset = (
                from guid in AssetDatabase.FindAssets($"{name} t:gameobject")
                let path = AssetDatabase.GUIDToAssetPath(guid)
                where Path.GetFileName(path) == filename
                let prefab = AssetDatabase.LoadAssetAtPath<T>(path)
                where prefab != null
                select new { prefab, path }
            ).FirstOrDefault();
            cache.Add(name, asset?.prefab.gameObject);
            return asset?.prefab.gameObject;
        }
	}
}