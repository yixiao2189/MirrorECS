using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjNameSystem : EgoSystem<EgoConstraint<ObjName>>
{
    public override void Update()
    {
        constraint.ForEachGameObject((eGo, entityName) => {
            eGo.gameObject.name = entityName.Name;

            if(entityName.isClientOnly)
                EgoHelper.RemoveComponent<ObjName>(eGo);
        });
    }
}
