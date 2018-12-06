using static Unity.Mathematics.math;
using Unity.Mathematics;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour {
	public Transform background;
	
	private float3 position;

	private void Start() {
		position = background.position - 2 * transform.position / 3;
	}

	private void Update() {
		var position = this.position;
		position.x += 2 * transform.position.x / 3;
		position.y += 2 * transform.position.y / 3;
		background.position = position;
	}
}