using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EgoSystem(EgoSide.ALL,EgoLayer.Front,-10)]
public class GameTimeSystem : EgoSystem<EgoConstraint<GameTime>>
{
    public static System.DateTime SVR_REL_TIME = System.DateTime.Parse("2023-01-01");

    public override void Start()
    {
        if (NetworkServer.active)
        {
            GameObject clone = UnityEngine.Object.Instantiate(ProjectNetworkManager.singleton.spawnPrefabs[0]);

            var eGo = EgoHelper.AddGameObject(clone);
            NetworkServer.Spawn(clone);

            EgoHelper.AddNetComponentOnServer<ObjName>(eGo).Name = $"GameTime";
            EgoHelper.AddNetComponentOnServer<GameTime>(eGo).startTime = (System.DateTime.UtcNow - SVR_REL_TIME).Ticks;
        }  
    }

    public override void Update()
    {
        constraint.ForEachGameObject((eGo,gameTime)=>
        {
            var timeVal = (float)System.TimeSpan.FromTicks((System.DateTime.UtcNow - SVR_REL_TIME).Ticks - gameTime.startTime).TotalSeconds;
            gameTime.SetTime((float)timeVal);        
        });
    }
}
