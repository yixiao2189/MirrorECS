
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ECSSystem : EgoInterface
{
    static ECSSystem Instance;

    public static ECSSystem GetInstance()
    {
        if (Instance == null)
        {
            GameObject gameObject = new GameObject("[ECS]");
            Instance = gameObject.AddComponent<ECSSystem>();
            DontDestroyOnLoad(gameObject);
        }
        return Instance;
    }

    bool _isStarted = false;

    void Awake()
    {
        Instance = this;
    }

    //Dont remove 
    public override void Start()
    {

    }

    public void Begin(NetworkManagerMode mode)
    {
        End();
        List<EgoSystem> list = new List<EgoSystem>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(EgoSystem)) && !type.IsAbstract && !type.IsGenericType)
                {
                    var egoAttr = (EgoSystemAttribute) type.GetCustomAttributes(false).Where(attri=>attri is EgoSystemAttribute).FirstOrDefault();
                    if (egoAttr == null || !egoAttr.acitve)
                        continue;
                    if (mode == NetworkManagerMode.Offline) continue;
                    if (mode == NetworkManagerMode.ClientOnly && egoAttr.side == EgoSide.SERVER)
                        continue;
                    if (mode == NetworkManagerMode.ServerOnly && egoAttr.side == EgoSide.CLIENT)
                        continue;

                    var ins =  System.Activator.CreateInstance(type) as EgoSystem;
                    ins.priority = (int)egoAttr.layer * 10000 + egoAttr.priority;
                    list.Add(ins);
                }
            }
        }
        list.Sort((x,y)=>x.priority.CompareTo(y.priority));
        EgoSystems.Add(list.ToArray());
    }



    public void End()
    {
        EgoSystems.Clear();
        EgoEvents.ClearAll();

        _isStarted = false;

    }

    public override void Update()
    {
        bool act = NetworkServer.active || NetworkClient.isConnected;
        if (!act)
            return;


        if (!_isStarted)
        {
            base.Start();
            _isStarted= true;
        }
        base.Update();
    }
}
