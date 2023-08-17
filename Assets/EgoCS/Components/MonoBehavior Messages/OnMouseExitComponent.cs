using UnityEngine;

[DisallowMultipleComponent]
public class OnMouseExitComponent : MonoBehaviour, IEgoComponent
{
	EgoComponent egoComponent;

	void Awake()
	{
		egoComponent = GetComponent<EgoComponent>();
	}

	void OnMouseExit()
	{
		var onMouseDownEvent = new MouseExitEvent( egoComponent );
		EgoEvents<MouseExitEvent>.AddEvent( onMouseDownEvent );
	}
}
