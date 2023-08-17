using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class LoadModel : NetworkBehaviour
{
    [SyncVar]
    public string resPath;
    public bool isInited = false;

}
