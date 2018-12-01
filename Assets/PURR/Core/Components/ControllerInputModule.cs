namespace PURR {
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Standard input module with mouse events disabled.</summary>
	[AddComponentMenu("# PURR/Controller Input Module")]
	public class ControllerInputModule : StandaloneInputModule {
		public override void Process() {
			bool usedEvent = SendUpdateEventToSelectedObject();
			if (eventSystem.sendNavigationEvents) {
				if (!usedEvent) {
					usedEvent |= SendMoveEventToSelectedObject();
				}
				if (!usedEvent) {
					SendSubmitEventToSelectedObject();
				}
			}
		}
	}
}