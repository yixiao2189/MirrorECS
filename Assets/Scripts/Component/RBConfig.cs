using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBConfig : NetworkBehaviour
{
    [SyncVar]
    public bool useGravity;
    [SyncVar]
    public float mass = 1;
}
