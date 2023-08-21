using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EgoSystem(EgoSide.CLIENT,EgoLayer.NONE)]
public class ModelSystem : EgoSystem<EgoConstraint<Model,AniState>>
{
    public override void Update()
    {
        constraint.ForEachGameObject((ego,model,aniState)=>
        { 
            model.anitor.SetBool("Moving", aniState.Moving);
        });
    }

}
