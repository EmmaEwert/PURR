using static Unity.Mathematics.math;
using UnityEngine;

public class PixelAligner : MonoBehaviour {
	public int pixelsPerUnit = 16;

	private void LateUpdate() {
		transform.position = round(transform.position * pixelsPerUnit) / pixelsPerUnit;
	}
}