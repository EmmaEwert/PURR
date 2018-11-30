namespace PURR.Tiled {
	///<summary>Miscellaneous `Array` helper methods</summary>
	internal static class ArrayExtension {
		///<summary>Reverse the row order in a 2D block of elements, assuming width `stride`.</summary>
		public static T[] Reverse<T>(this T[] input, int stride) {
			var size = input.Length / stride;
			var output = new T[input.Length];
			for (var row = 0; row < size; ++row) {
				for (var col = 0; col < stride; ++col) {
					var i = row * stride + col;
					var j = (size - row - 1) * stride + col;
					output[i] = input[j];
				}
			}
			return output;
		}

		///<summary>Get a 2D block of elements, assuming width `stride`.</summary>
		public static T[] Block<T>(this T[] input, int x, int y, int width, int height, int stride) {
			var size = input.Length / stride;
			var output = new T[width * height];
			for (var row = 0; row < height; ++row) {
				for (var col = 0; col < width; ++col) {
					var i = row * width + col;
					var j = (size - y - row - height) * stride + x + col;
					output[i] = input[j];
				}
			}
			return output;
		}
	}
}
