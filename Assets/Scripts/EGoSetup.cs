using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EGoSetup : NetworkBehaviour
{
    static EGoSetup()
    {
        NetworkServer.DestroyAction = (gameObject) =>
        {
            if (gameObject == null) return;
            var comp = gameObject.GetComponent<EgoComponent>();
            if (comp != null)
            {
                Ego.DestroyGameObject(comp);
            }
            else
            {
                Destroy(gameObject); 
            }          
        };
        NetworkIdentity.OnInitComponent = (gameObject) =>
        {
            EgoHelper.AddGameObject(gameObject);
        };

        NetworkIdentity.OnRecvAddComponent = (gameObject, type) =>
        {
            var eGo = gameObject.GetComponent<EgoComponent>();
            if (eGo == null)
                return null;
            return  Ego.AddComponent(eGo, type);
        };

        NetworkIdentity.OnRecvRmComponent = (gameObject, type) =>
        {
            var eGo = gameObject.GetComponent<EgoComponent>();
            if (eGo == null)
                return false;
            return Ego.DestroyComponent(eGo, type);
        };
    }

    public override void OnStartLocalPlayer()
    {
        gameObject.AddComponent<BattleInput>();
    }

    [Command]
    public void AddNetworkBehavior(string strType)
    {
        var type = System.Type.GetType(strType);
        if (type == null)
            throw new System.Exception($"AddNetworkBehavior {strType}");
        var eGo = GetComponent<EgoComponent>();
        if (eGo == null) return;
        var comp =  Ego.AddComponent(eGo, type);
        netIdentity.AddNetworkBehaviour(comp as NetworkBehaviour);
    }

    [Command]
    public void RemoveNetworkBehavior(string strType)
    {
        var type = System.Type.GetType(strType);
        if (type == null)
            throw new System.Exception($"RemoveNetworkBehavior {strType}");
        var comp = gameObject.GetComponent(type);
        if (comp == null)
            return;
        netIdentity.RemoveNetworkBehaviour(comp as NetworkBehaviour);

        var eGo = GetComponent<EgoComponent>();
        Ego.DestroyComponent(eGo, type);
    }
}
