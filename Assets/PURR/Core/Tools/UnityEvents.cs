namespace PURR {
	using System;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;

	///<summary>Event for calling methods with a dynamic `bool` parameter.</summary>
	[Serializable] public class BoolEvent : UnityEvent<bool> { }

	///<summary>Event for calling methods with a dynamic `MoveDirection` parameter.</summary>
	[Serializable] public class DirectionEvent : UnityEvent<MoveDirection> { }
}