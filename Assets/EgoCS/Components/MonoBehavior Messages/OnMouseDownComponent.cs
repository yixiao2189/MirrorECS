using UnityEngine;

[DisallowMultipleComponent]
public class OnMouseDownComponent : MonoBehaviour, IEgoComponent
{
	EgoComponent egoComponent;

	void Awake()
	{
		egoComponent = GetComponent<EgoComponent>();
	}

	void OnMouseDown()
	{
		var onMouseDownEvent = new MouseDownEvent( egoComponent );
		EgoEvents<MouseDownEvent>.AddEvent( onMouseDownEvent );
	}
}
