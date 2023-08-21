using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveSystem : EgoSystem<EgoConstraint<Velocity>>
{
    public override void Update()
    {
        constraint.ForEachGameObject((eGo, comp) => {
           var transform = eGo.transform;
           transform.Rotate(0, comp.dir.x * 100 * Time.deltaTime, 0);
            float y = Mathf.Max(comp.dir.y, 0);
           transform.Translate(Vector3.forward * y * Time.deltaTime, Space.Self);

            EgoHelper.AddNetComponentOnServer<AniState>(eGo).Moving = y != 0;
        });
    }
}
