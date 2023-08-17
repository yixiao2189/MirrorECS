using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class ObjName : NetworkBehaviour
{
    [SyncVar]
    public string Name;
}
