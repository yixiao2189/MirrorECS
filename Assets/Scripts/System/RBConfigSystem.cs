using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBConfigSystem : EgoSystem<EgoConstraint<RBConfig>>
{
    public override void Update()
    {
        constraint.ForEachGameObject((eGo,config)=>
        { 
            var go = eGo.gameObject;
            var comp = Ego.AddComponent<Rigidbody>(eGo);
            comp.useGravity = config.useGravity;
            comp.mass = config.mass;

            EgoHelper.RemoveComponent<RBConfig>(eGo);
        });
    }
}
