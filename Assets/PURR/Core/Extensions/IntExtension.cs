using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PURR {
	public static class IntExtension {
		public static TaskAwaiter GetAwaiter(this int milliseconds) =>
			Task.Delay(milliseconds).GetAwaiter();
	}
}