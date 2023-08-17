using Noah.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTriggerSystem : EgoSystem<EgoConstraint<Bullet,PhCollisionContainer>>
{
    public override void Update()
    {
        constraint.ForEachGameObject((ego, bullet, container) => {
            var list = container.Pop(PhysicsEvent.Enter);
            if(list == null) return;
            foreach (var e in list)
            {
                Debug.Log($"On hit: {e.frameId} {e.egoComponent1} {e.collider}  ");
                EgoHelper.DestroyGameObject(ego.gameObject);
            }
        });
    }

}
