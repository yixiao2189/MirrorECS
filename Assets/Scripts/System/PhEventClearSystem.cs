using Noah.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EgoSystem(EgoSide.SERVER,EgoLayer.Back,10)]
public class PhEventClearSystem : EgoSystem<EgoConstraint<PhCollisionContainer>>
{
    public override void Update()
    {
        constraint.ForEachGameObject((ego, container) => {
            container.ClearOutFrame(Time.frameCount);
        });
    }
}
