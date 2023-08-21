using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class EgoSystems
{
    static List<EgoSystem> _systems = new List<EgoSystem>();
    public static List<EgoSystem> systems { get { return _systems; } }

    public static void Add(IEnumerable<EgoSystem> systems )
    {
        _systems.AddRange(systems);
    }

    public static void Clear()
    {
        _systems.Clear();
    }

    public static void Start()
    {
        EgoEvents.Start();

        // Start all Systems
        foreach( var system in _systems )
        {
            system.Start();
        }

        // Invoke all queued Events
        EgoEvents.Invoke();

        // Clean up Destroyed Components & GameObjects
        EgoCleanUp.CleanUp();
    }

    /// <summary>
    /// Attaches and Initializes an EgoComponent on the given transform
    /// and all of its children (recursively)
    /// </summary>
    /// <param name="transform"></param>
    static void InitEgoComponent( GameObject gameObject )
    {
        var egoComponent = gameObject.GetComponent<EgoComponent>();
        if( egoComponent == null ) { egoComponent = gameObject.AddComponent<EgoComponent>(); }
        egoComponent.CreateMask();

        var transform = gameObject.transform;
        var childCount = transform.childCount;
        for( var i = 0; i < childCount; i++ )
        {
            InitEgoComponent( transform.GetChild( i ).gameObject );
        }       
    }

    public static void Update()
    {
        // Update all Systems
        foreach( var system in _systems )
        {
#if UNITY_EDITOR
            if ( system.enabled ) system.Update();
#else
            system.Update();
#endif
        }

        // Invoke all queued Events
        EgoEvents.Invoke();

        // Clean up Destroyed Components & GameObjects
        EgoCleanUp.CleanUp();
    }

    public static void FixedUpdate()
    {
        // Update all Systems
        foreach( var system in _systems )
        {
#if UNITY_EDITOR
            if( system.enabled ) system.FixedUpdate();
#else
            system.FixedUpdate();
#endif            
        }
    }
}
