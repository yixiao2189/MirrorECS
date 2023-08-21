using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EgoSystem(EgoSide.ALL)]
public class RBMoveSystem : EgoSystem<EgoConstraint<Rigidbody,RBMove>>
{
    public override void Update()
    {
        constraint.ForEachGameObject((eGo, rigidbody, move) => {
            rigidbody.AddForce(move.force);
            
            EgoHelper.RemoveComponent<RBMove>(eGo);
        });
    }
}
