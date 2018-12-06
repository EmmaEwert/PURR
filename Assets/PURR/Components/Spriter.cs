namespace PURR {
	using static Unity.Mathematics.math;
	using UnityEngine;
	using UnityEngine.Tilemaps;

	///<summary>Assigns sprite on import.</summary>
	public class Spriter : Component {
#pragma warning disable CS0108 // hides inherited member
		private SpriteRenderer renderer => GetComponentInChildren<SpriteRenderer>();
#pragma warning restore CS0108

		public override void OnImportObject(Object obj) {
			if (obj.tile?.sprite) {
				var renderer = new GameObject("Renderer").AddComponent<SpriteRenderer>();
				renderer.sprite = obj.tile.sprite;
				renderer.transform.SetParent(transform, worldPositionStays: false);
				renderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			}
		}
	}
}