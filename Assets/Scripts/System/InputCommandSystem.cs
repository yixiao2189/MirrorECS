using Mirror;
using Noah.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EgoSystem(EgoSide.SERVER,EgoLayer.Front)]
public class InputCommandSystem : EgoSystem<EgoConstraint<InputCommand>>
{
    public override void Update()
    {
        constraint.ForEachGameObject((eGo,commad)=>
        {
            var inputMove = commad.input;
            var transform = eGo.transform;



            if (commad.isFire)
            {
                commad.isFire = false;

                Fire(eGo.transform,commad.netIdentity);
            }
        });
    }

    void Fire(Transform root,NetworkIdentity networkIdentity)
    {
        var model = root.GetComponent<Model>();
        if (model == null) return;
        if (model.mountNode == null) return;

        GameObject clone = Object.Instantiate(ProjectNetworkManager.singleton.spawnPrefabs[0], model.mountNode.fireNode.position, root.transform.rotation);

        var eGo = EgoHelper.AddGameObject(clone);
        NetworkServer.Spawn(clone);

        EgoHelper.AddNetComponentOnServer<ObjName>(eGo).Name = $"Projectile @{networkIdentity.netId}";
        EgoHelper.AddNetComponentOnServer<LoadModel>(eGo).resPath = "Projectile";
    
        EgoHelper.AddNetComponentOnServer<RBMove>(eGo).force = root.transform.forward * 1000;
        EgoHelper.AddNetComponentOnServer<RBConfig>(eGo).useGravity = false;
        EgoHelper.AddNetComponentOnServer<ColliderConfig>(eGo).radius = 0.05f;
        var lifeTime = EgoHelper.AddNetComponentOnServer<LifeTime>(eGo);
        lifeTime.startTime = GameTime.gameTime;
        lifeTime.duraTime = 5;
        EgoHelper.AddComponent<PhCollisionContainer>(eGo);
        EgoHelper.AddNetComponentOnServer<Bullet>(eGo);
    }

    void MoveLogic(Transform transform,Vector2 inputMove)
    {
        var agent = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();
        transform.Rotate(0, inputMove.x * 100 * Time.deltaTime, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (agent != null)
            agent.velocity = forward * Mathf.Max(inputMove.y, 0) * agent.speed;
    }
}
