using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColliderConfigSystem : EgoSystem<EgoConstraint<ColliderConfig>>
{
    public override void Update()
    {
        constraint.ForEachGameObject((eGo, config) =>
        {
            var go = eGo.gameObject;
            var comp = Ego.AddComponent<SphereCollider>(eGo);
            comp.radius = config.radius;

            EgoHelper.RemoveComponent<ColliderConfig>(eGo);


        });
    }
}
