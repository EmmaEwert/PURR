namespace PURR {
	using UnityEngine.EventSystems;

	///<summary>Standard input module with mouse events disabled.</summary>
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