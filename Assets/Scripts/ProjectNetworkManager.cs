using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Video;

public class ProjectNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn)
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

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        GetECSStartup().BeginECS();
        GetECSStartup().BeginClient();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);


        GetECSStartup().EndECS();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        GetECSStartup().BeginECS();
        GetECSStartup().BeginServer();
    }

}
