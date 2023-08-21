using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[EgoSystem(EgoSide.ALL,EgoLayer.Front,0)]
public class LoadModelSystem : EgoSystem<EgoConstraint<LoadModel,NetworkIdentity>>
{
    public override void Update()
    {
        constraint.ForEachGameObject((eGoComponent, loadModel, netID) =>
        {
            if (loadModel.isInited || string.IsNullOrEmpty(loadModel.resPath))
                return;

            loadModel.isInited = true;


            //OnLoaded....
            Transform netTransform = eGoComponent.transform;
            var go = GameObject.Instantiate(Resources.Load(loadModel.resPath) as GameObject) ;
    
            go.transform.SetParent(netTransform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;


            if (netID.isClientOnly)      
                EgoHelper.RemoveComponent<LoadModel>(eGoComponent);



            //赋值，但是Server无ModelSystem
            var model = EgoHelper.AddComponent<Model>(eGoComponent);
            model.root = eGoComponent.gameObject;
            model.viewGO = go;
            model.anitor = go.GetComponent<Animator>();
            model.mountNode = go.GetComponent<MountNode>();


            CopyCompoentTo(go,netTransform.gameObject,true);
        });
    }

    public void CopyCompoentTo(GameObject source,GameObject target,bool disableSource = false)
    {
        var navAgentS = source.GetComponent<NavMeshAgent>();
        if (navAgentS != null)
        {
            navAgentS.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

            var navAgentT = target.GetComponent<NavMeshAgent>();
            if(navAgentT == null)
                navAgentT = target.AddComponent<NavMeshAgent>();
            navAgentT.speed = navAgentS.speed;  
            navAgentT.angularSpeed= navAgentS.angularSpeed;
            navAgentT.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }

        var sphereColliderS = source.GetComponent<SphereCollider>();
        if (sphereColliderS != null)
        {
            var sphereColliderT = target.GetComponent<SphereCollider>();
            if (sphereColliderT == null)
                sphereColliderT = target.AddComponent<SphereCollider>();
            sphereColliderT.radius = sphereColliderS.radius;
            sphereColliderT.center = sphereColliderS.center;
            sphereColliderS.isTrigger = sphereColliderT.isTrigger;
        }

        if (disableSource)
        {
            if(navAgentS != null)
                navAgentS.enabled = false;
            if (sphereColliderS != null)
                sphereColliderS.enabled = false;
        }
    }
}
