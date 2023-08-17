using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : NetworkBehaviour
{
    [SyncVar]
    public float startTime;
    [SyncVar]
    public float duraTime;
}
