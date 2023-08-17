using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine.AI;

public static class ComponentIDs_Unity
{
    public static readonly List<Type> componentTypes = new List<Type>()
    {
        typeof(EgoComponent),
        typeof(Transform),
        typeof(Renderer),
        typeof(Collider),
        typeof(MeshFilter),
        typeof(Rigidbody),
        typeof(NavMeshAgent),
        typeof(Mirror.NetworkIdentity)
    };
}

public interface IEgoComponent
{
}


public static class ComponentIDs
{
    public static readonly List<Type> componentTypes;
    private static readonly Dictionary<Type, int> _types;

    static ComponentIDs()
    {
        // Get all Component Types
        componentTypes = new List<Type>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach( var assembly in assemblies )
        {
            var types = assembly.GetTypes();
            foreach( var type in types )
            {
                if( type.IsSubclassOf( typeof( Component ) ) && !type.IsAbstract )
                {
                    if(typeof(IEgoComponent).IsAssignableFrom(type))
                        componentTypes.Add( type );
                    if (typeof(NetworkBehaviour).IsAssignableFrom(type))
                        componentTypes.Add(type);
                    if (ComponentIDs_Unity.componentTypes.Contains(type))
                        componentTypes.Add(type);
                    else
                    {
                        foreach (var uType in ComponentIDs_Unity.componentTypes)
                        { 
                            if (uType.IsAssignableFrom(type))
                            {
                                componentTypes.Add(type);
                            }
                        }
                    }
                }
            }
        }

        _types = new Dictionary<Type, int>( componentTypes.Count );
        foreach( var componentType in componentTypes )
        {
            _types.Add( componentType, _types.Count );
        }
    }

    public static int GetCount()
    {
        return _types.Count;
    }

    public static int Get( Type componentType )
    {
        Assert.IsTrue( componentType.IsSubclassOf( typeof( Component ) ), "Only get IDs of Component Types!" );
        return _types[ componentType ];
    }
}
