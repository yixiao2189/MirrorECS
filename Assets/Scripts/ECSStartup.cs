using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ECSStartup : EgoInterface
{
    bool isAddComman = false;
    bool isStarted = false;

    public void BeginECS()
    {
        if (isAddComman)
            return;
        EndECS();
        EgoSystems.Add(
            new GameTimeSystem(),
            new BulletTriggerSystem(),
           new ObjNameSystem(),
           new LoadModelSystem(),
           new RBConfigSystem(),
           new ColliderConfigSystem(),
           new RBMoveSystem(),
           new PhEventClearSystem()
       );
       isAddComman = true;
    }

    public void BeginServer()
    {
        EgoSystems.Add(
            new LifeTimeSystem(),
           new InputCommandSystem(),
           new MoveSystem()
       );
    }

    public void BeginClient()
    {
        EgoSystems.Add(
           new ModelSystem()
       );
    }

    public void EndECS()
    {
        EgoSystems.Clear();
        EgoEvents.ClearAll();
        isAddComman = false;
        isStarted = false;

    }

    public override void Update()
    {
        bool act = NetworkServer.active || NetworkClient.isConnected;
        if (!act)
            return;


        if (!isStarted)
        {
            base.Start();
            isStarted= true;
        }
        base.Update();
    }
}
