using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[EgoSystem(EgoSide.SERVER)]
public class LifeTimeSystem : EgoSystem<EgoConstraint<LifeTime>>
{
    public override void Update()
    {
        constraint.ForEachGameObject((eGo, lifeTime) => {
            if (GameTime.gameTime - lifeTime.startTime > lifeTime.duraTime)
            {
                NetworkServer.Destroy(eGo.gameObject);
            }
        });
    }
}
