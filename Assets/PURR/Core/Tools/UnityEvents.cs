namespace PURR {
	using System;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;

	///<summary>Event for calling methods with a dynamic `MoveDirection` parameter.</summary>
	[Serializable] public class DirectionEvent : UnityEvent<MoveDirection> { }
}