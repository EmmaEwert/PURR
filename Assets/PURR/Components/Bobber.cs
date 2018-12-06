namespace PURR {
	using static Unity.Mathematics.math;
	using Unity.Mathematics;
	using UnityEngine;

	///<summary>Bobs transform. Used by textbox arrow.</summary>
	public class Bobber : Component {
		float3 position;
		private void OnEnable() => position = transform.position;
		private void OnDisable() => transform.position = position;
		private void Update() => transform.position = position + float3(0, abs(sin(Time.time * 8)) * 2, 0);
	}
}