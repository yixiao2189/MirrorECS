using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoComponentRegister : IEgoComponentRegister
{
    static readonly List<Type> types = new List<Type>
    {
        typeof(NetworkIdentity),
        typeof(NetworkBehaviour)
    };

    public List<Type> GetTypes()
    {
        return types;
    }
}

public static class EgoHelper 
{
    public static EgoComponent AddGameObject(GameObject gameObject)
    {
        var eGo = gameObject.GetComponent<EgoComponent>();
        if (eGo == null)
            eGo = Ego.AddGameObject(gameObject);
        return eGo;
    }

    public static void DestroyGameObject(GameObject go)
    {
        NetworkServer.Destroy(go);
    }

    public static C AddComponent<C>(EgoComponent egoComponent) where C : Component, IEgoComponent
    {
        return Ego.AddComponent<C>(egoComponent); 
    }

    public static bool DestroyComponent<C>(EgoComponent egoComponent) where C : Component, IEgoComponent
    {
        return Ego.DestroyComponent<C>(egoComponent);
    }

    public static bool RemoveComponent<C>(EgoComponent egoComponent) where C : Component
    {
        C component = null;
        if (!egoComponent.TryGetComponents<C>(out component)) { return false; }

        var e = new DestroyedComponent<C>(component, egoComponent);
        EgoEvents<DestroyedComponent<C>>.AddEvent(e);
        EgoCleanUp<C>.Destroy(egoComponent, component,false);
        return true;
    }

    public static C AddNetComponentOnServer<C>(EgoComponent egoComponent) where C : NetworkBehaviour
    {
        C component = null;
        if (!egoComponent.TryGetComponents<C>(out component))
        {
            component = egoComponent.gameObject.AddComponent<C>();
            egoComponent.mask[ComponentIDs.Get(typeof(C))] = true;
            EgoEvents<AddedComponent<C>>.AddEvent(new AddedComponent<C>(component, egoComponent));

            //net
            var netIdentity = egoComponent.gameObject.GetComponent<NetworkIdentity>();
            if (netIdentity != null)
                netIdentity.AddNetworkBehaviour(component);
            //
        }

        return component;
    }

    public static bool DestroyNetComponentOnServer<C>(EgoComponent egoComponent) where C : NetworkBehaviour
    {
        C component = null;
        if (!egoComponent.TryGetComponents<C>(out component)) { return false; }

        //net
        var netIdentity = egoComponent.gameObject.GetComponent<NetworkIdentity>();
        if (netIdentity != null)
            netIdentity.RemoveNetworkBehaviour(component as NetworkBehaviour);
        //

        var e = new DestroyedComponent<C>(component, egoComponent);
        EgoEvents<DestroyedComponent<C>>.AddEvent(e);
        EgoCleanUp<C>.Destroy(egoComponent, component);
        return true;
    }

    //Client Call Server as Command
    public static C AddNetComponentCommad<C>(EgoComponent egoComponent) where C : NetworkBehaviour
    {
        C component = null;
        if (!egoComponent.TryGetComponents<C>(out component))
        {
            component = egoComponent.gameObject.AddComponent<C>();
            egoComponent.mask[ComponentIDs.Get(typeof(C))] = true;
            EgoEvents<AddedComponent<C>>.AddEvent(new AddedComponent<C>(component, egoComponent));

            //net
            egoComponent.GetComponent<EGoSetup>().AddNetworkBehavior(typeof(C).FullName);
            //
        }

        return component;
    }

    //Client Call Server as Command
    public static bool DestroyNetComponentCommand<C>(EgoComponent egoComponent) where C : NetworkBehaviour
    {
        C component = null;
        if (!egoComponent.TryGetComponents<C>(out component)) { return false; }

        //net
        egoComponent.GetComponent<EGoSetup>().RemoveNetworkBehavior(typeof(C).FullName);
        //

        var e = new DestroyedComponent<C>(component, egoComponent);
        EgoEvents<DestroyedComponent<C>>.AddEvent(e);
        EgoCleanUp<C>.Destroy(egoComponent, component);
        return true;
    }

}
