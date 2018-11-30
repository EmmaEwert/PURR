namespace PURR.Tiled {
    using System.Collections.Generic;
    using System.Diagnostics;
    using UnityEngine;

	///<summary>Benchmark utilities, mainly stopwatch functionality.</summary>
    internal static class Benchmark {
#if DEBUG_PURR
        private static Dictionary<string, Stopwatch> watches = new Dictionary<string, Stopwatch>();

		///<summary>Start or resume a named benchmark timer.</summary>
        public static void StartWatch(string name) {
            if (watches.Count == 0) {
                watches.Add(name, Stopwatch.StartNew());
            } else if (watches.TryGetValue(name, out var watch)) {
				watch.Start();
			} else {
				watches.Add(name, Stopwatch.StartNew());
			}
        }

		///<summary>Stop or pause a named benchmark timer.</summary>
        public static void StopWatch(string name) {
			if (watches.TryGetValue(name, out var watch)) {
				watch.Stop();
			}
        }

		///<summary>Stop all running watches, and print their names and values to the log.</summary>
        public static void StopWatches() {
			const int targetMilliseconds = 500; // milliseconds
            var log = string.Empty;
            var green = new Color(0.125f, 0.75f, 0f);
            var red = new Color(0.875f, 0.125f, 0f);
            foreach (var w in watches) {
                var milliseconds = w.Value.Elapsed.TotalMilliseconds;
                var c = Color32.Lerp(green, Color.red, (float)(milliseconds / targetMilliseconds));
                log +=
					$"<b><color=\"#{c.r:x2}{c.g:x2}{c.b:x2}ff\">"
					+ $"{milliseconds:0000.0000}</color></b> milliseconds"
					+ $" : <b>{w.Key}</b>\n";
            }
            watches.Clear();
			UnityEngine.Debug.Log(log);
        }
#else
        public static void StartWatch(string name) {}
        public static void StopWatch(string name = null) {}
        public static void StopWatches() {}
#endif // DEBUG_PURR
    }
}