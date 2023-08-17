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


    ECSStartup GetECSStartup()
    {
        var comp = gameObject.GetComponent<ECSStartup>();
        if (comp == null)
            comp = gameObject.AddComponent<ECSStartup>();
        return comp;
    }

    public override void OnClientDisconnect()
    {


        GetECSStartup().EndECS();
        base.OnClientDisconnect();
    }

    public override void OnClientConnect()
    {
    
        GetECSStartup().BeginECS();
        GetECSStartup().BeginClient();
        base.OnClientConnect();
    }



    public override void OnStartServer()
    {

        GetECSStartup().BeginECS();
        GetECSStartup().BeginServer();

        base.OnStartServer();
    }

}
