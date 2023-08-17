using UnityEngine;

[RequireComponent( typeof( EgoComponent ) ) ]
public class OnTriggerEnter2DComponent : MonoBehaviour, IEgoComponent
{
    EgoComponent egoComponent;

    void Awake()
    {
        egoComponent = GetComponent<EgoComponent>();
    }

    void OnTriggerEnter2D( Collider2D collider2d )
    {
        var e = new TriggerEnter2DEvent( egoComponent, collider2d.gameObject.GetComponent<EgoComponent>(), collider2d );
        EgoEvents<TriggerEnter2DEvent>.AddEvent( e );
    }
}
