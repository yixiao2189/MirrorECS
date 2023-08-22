using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;
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
    };
}

public interface IEgoComponentRegister
{
    List<Type> GetTypes();
}


public interface IEgoComponent
{
}


public static class ComponentIDs
{
    static readonly List<Type> componentTypes;
    static readonly Dictionary<Type, int> _types;

    static readonly List<Type> _registerTypes;

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
                if (type.IsAbstract || type.IsInterface) continue;
                if (type.IsSubclassOf(typeof(Component)))
                {
                    componentTypes.Add(type);
                } else if (typeof(IEgoComponentRegister).IsAssignableFrom(type))
                {
                    var ins = System.Activator.CreateInstance(type) as IEgoComponentRegister;
                    ComponentIDs_Unity.componentTypes.AddRange(ins.GetTypes());
                }
            }
        }

        int count = componentTypes.Count;
        for (int i = 0; i <count;i++)
        {
            var type = componentTypes[i];   
            if (typeof(IEgoComponent).IsAssignableFrom(type))
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

        componentTypes.RemoveRange(0, count); 

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
