using UnityEngine;

[DisallowMultipleComponent]
public class OnMouseUpAsButtonComponent : MonoBehaviour, IEgoComponent
{
	EgoComponent egoComponent;

	void Awake()
	{
		egoComponent = GetComponent<EgoComponent>();
	}

	void OnMouseUpAsButton()
	{
		var onMouseDownEvent = new MouseUpAsButtonEvent( egoComponent );
		EgoEvents<MouseUpAsButtonEvent>.AddEvent( onMouseDownEvent );
	}
}
