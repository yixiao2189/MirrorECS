using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
