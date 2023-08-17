using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class Velocity : NetworkBehaviour
{
    [SyncVar]
    public Vector3 dir;

    [SyncVar]
    public float speed;
}
