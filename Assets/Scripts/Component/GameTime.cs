using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : NetworkBehaviour
{
    static GameTime Instance = null;

    [SyncVar]
    public long startTime;
    public float timeVal;

    void Awake()
    {
        Instance = this;
    }

    public static float gameTime
    {
        get {
            if (Instance == null)
                return 0;
            return Instance.timeVal;
        }
    }

    public void SetTime(float value)
    {
        timeVal = value;
    }
}
