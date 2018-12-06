using static Unity.Mathematics.math;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class SelectedFramer : MonoBehaviour {
	public float2 target = float2(0.25f, 0.75f);
#pragma warning disable CS0108 // hides inherited member
	private Camera camera => GetComponent<Camera>();
#pragma warning restore CS0108

	private void Update() {
		var position = EventSystem.current?.currentSelectedGameObject?.transform.position ?? default(Vector3);

		// Keep selected object within window? Maybe?
		var viewOffset = camera.WorldToViewportPoint(position);
		viewOffset.x =
			min(viewOffset.x, 1 - target.x / 2) - target.x / 2
			+ max(viewOffset.x, target.x / 2) - target.x / 2;
		viewOffset.y = 
			min(viewOffset.y, 1 - target.y / 2) - target.y / 2
			+ max(viewOffset.y, target.y / 2) - target.y / 2;
		var worldOffset = camera.ViewportToWorldPoint(viewOffset);
		worldOffset.z = transform.position.z;
		transform.position = lerp(transform.position, worldOffset, Time.deltaTime);

		// Keep camera within tilemap bounds? Maybe?
		var bottomLeftOffset = -camera.ViewportToWorldPoint(float3(0));
		bottomLeftOffset.x = max(0, bottomLeftOffset.x);
		bottomLeftOffset.y = max(0, bottomLeftOffset.y);
		bottomLeftOffset.z = 0;
		transform.position += bottomLeftOffset;
	}
}
