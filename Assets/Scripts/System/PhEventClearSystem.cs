using Noah.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhEventClearSystem : EgoSystem<EgoConstraint<PhCollisionContainer>>
{
    public override void Update()
    {
        constraint.ForEachGameObject((ego, container) => {
            container.ClearOutFrame(Time.frameCount);
        });
    }
}
