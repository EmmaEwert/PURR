namespace PURR {
	using UnityEngine.Events;
	using UnityEngine.EventSystems;

	///<summary>Runs delegates when submit is pressed.</summary>
	public class SubmitHandler : Component, ISubmitHandler {
		public UnityEvent onSubmit;

		public void OnSubmit(BaseEventData _) {
			onSubmit.Invoke();
		}
	}
}