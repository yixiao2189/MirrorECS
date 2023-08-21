using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Video;

public class ProjectNetworkManager : NetworkManager
{

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {

        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);


      
        NetworkServer.AddPlayerForConnection(conn, player);

        var eGo = EgoHelper.AddGameObject(player);
        EgoHelper.AddNetComponentOnServer<ObjName>(eGo).Name = $"Player netId={conn.identity.netId} [connId={conn.connectionId}] ";

        EgoHelper.AddNetComponentOnServer<LoadModel>(eGo).resPath = "Tank";
        EgoHelper.AddNetComponentOnServer<Player>(eGo);
    }



    public override void OnClientDisconnect()
    {

        if (mode != NetworkManagerMode.ClientOnly)
            return;


        ECSSystem.GetInstance().End();
    }

    public override void OnStopServer()
    {

        ECSSystem.GetInstance().End();
    }


    public override void OnStart()
    {
        ECSSystem.GetInstance().Begin(mode);
    }

}
